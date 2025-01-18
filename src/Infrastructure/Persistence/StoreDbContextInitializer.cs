using System.Text.Json;
using Bogus;
using eCommerceWeb.Application.Abstractions.Database;
using eCommerceWeb.Domain.Entities.CatalogAggregate;
using eCommerceWeb.Domain.Entities.Directory;
using eCommerceWeb.Domain.Entities.Misc;
using Microsoft.Extensions.Logging;
using SharedKernel.Utilities;

namespace eCommerceWeb.Persistence;

internal sealed partial class StoreDbContextInitializer(
    StoreDbContext storeDbContext,
    ILogger<StoreDbContextInitializer> logger) : IDatabaseInitializer
{
    private static readonly Faker _faker = new();

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await storeDbContext.Database.MigrateAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Error while initializing database '{Dbcontext}' in {Initializer}.", 
                nameof(StoreDbContext), 
                nameof(StoreDbContextInitializer)
            );
            throw;
        }
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            using var tx = await storeDbContext.Database.BeginTransactionAsync(cancellationToken);

            //await DbCleanUpAsync(cancellationToken);
            await SeedDirectory(cancellationToken);
            await SeedProduct(cancellationToken);

            await storeDbContext.SaveChangesAsync(cancellationToken);
            await tx.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Error while seeding database '{Dbcontext}' in {Initializer}.", 
                nameof(StoreDbContext), 
                nameof(StoreDbContextInitializer)
            );
            throw;
        }
    }

    private async Task DbCleanUpAsync(CancellationToken cancellationToken)
    {
        await storeDbContext.Set<Category>().ExecuteDeleteAsync(cancellationToken);
        await storeDbContext.Set<Product>().ExecuteDeleteAsync(cancellationToken);
        await storeDbContext.Set<ProductTag>().ExecuteDeleteAsync(cancellationToken);
        await storeDbContext.Set<MediaFile>().ExecuteDeleteAsync(cancellationToken);
    }
}

#region Catalog
internal partial class StoreDbContextInitializer
{
    private static readonly (string Name, string ImageUrl, bool OnSale, decimal UnitPrice, decimal SalePrice)[] products = [
        ("Blue Shoes", "", false, 36.50m, 0m),
        ("Black Shoes", "", false, 40m, 0m),
        ("White Shirt", "", true, 12.75m, 10.25m),
        ("Blue Shirt", "", false, 10m, 0m),
        ("Grey Joggers", "", true, 21.75m, 15.55m),
        ("Baseball Cap", "", false, 15.90m, 0m)
    ];

    private async Task SeedProduct(CancellationToken cancellationToken)
    {
        var productContext = storeDbContext.Set<Product>();
        if (await productContext.AnyAsync(cancellationToken)) return;

        var visibilities = Visibility.ListNames().Take(2).ToList();

        // int i = 0;
        foreach (var (name, imageUrl, onSale, unitPrice, salePrice) in products)
        {
            var isUnlimited = _faker.Random.Bool();
            var model = new ProductCreationModel(
                Sku: $"SQ{_faker.Random.Int(1001111, 9999109)}",
                Name: name,
                Description: string.Join(Environment.NewLine, _faker.Lorem.Sentences(_faker.Random.Int(1, 5))),
                OnSale: onSale,
                UnitPrice: unitPrice,
                SalePrice: salePrice,
                StockQuantity: isUnlimited ? null : _faker.Random.Int(1, 25),
                HasUnlimitedStock: isUnlimited,
                IsFeatured: _faker.Random.Bool(),
                EnableProductReviews: true,
                EnableRelatedProducts: false,
                UrlSlug: SeoHelper.GenerateUrlSlug(name),
                MetaTitle: _faker.Lorem.Sentence(3),
                MetaKeywords: null,
                MetaDescription: null,
                SocialImageUrl: null
            );

            var product = new Product(model);
            product.SetVisibility(_faker.PickRandom(visibilities));

            if (!string.IsNullOrWhiteSpace(imageUrl))
            {
                // var bytes = await LoadFileAsync(imageUrl, cancellationToken);
                // if (bytes.Length != 0)
                // {
                //     using var stream = new MemoryStream(bytes);
                //     var mediaFile = new MediaFile(FileType.Image(), $"IMG_{++i}", "image/jpg", stream.Length);

                //     var response = await fileStorageManger.UploadAsync(mediaFile, stream, cancellationToken);
                    
                //     mediaFile.SetLocation(response.FileId);
                //     product.AddImage(new(mediaFile.FileLocation, name, true, 0));

                //     await storeDbContext.AddAsync(mediaFile, cancellationToken);
                // }
            }

            await productContext.AddAsync(product, cancellationToken);
        }
    }

    private async Task<byte[]> LoadFileAsync(string path, CancellationToken cancellationToken)
    {
        byte[] content = [];

        try
        {
            using var client = new HttpClient() { Timeout = TimeSpan.FromSeconds(15) };
            content = path.StartsWith("http") switch
            {
                true => await client.GetByteArrayAsync(new Uri(path).GetLeftPart(UriPartial.Path), cancellationToken),
                _ => await File.ReadAllBytesAsync(path, cancellationToken),
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex,  "Failed to load file path: {Path}", path);
        }

        return content;
    }
}
#endregion

# region Directory
internal partial class StoreDbContextInitializer
{
    private static readonly List<Currency> currencies = [
        new("US Dollar", "USD", "$"),
        new("British Pound", "GBP", "£"),
        new("Euro", "EUR", "€"),
        new("Swiss Franc", "CHF", "Fr."),
        new("Canadian Dollar", "CAD", "$"),
        new("Australian Dollar", "AUD", "$"),
        new("Indian Rupee", "INR", "₹"),
        new("Nigerian Naira", "NGN", "₦"),
        new("Mexican Peso", "MXN", "$"),
        new("Japanese Yen", "JPY", "¥"),
        new("South African Rand", "ZAR", "R"),
    ];

    private async Task SeedDirectory(CancellationToken cancellationToken)
    {
        await SeedCountry();
        await SeedCurrency();

        async Task SeedCurrency()
        {
            var currencyContext = storeDbContext.Set<Currency>();
            if (await currencyContext.AnyAsync(cancellationToken)) return;

            await currencyContext.AddRangeAsync(currencies, cancellationToken);
        }

        async Task SeedCountry()
        {
            var countryContext = storeDbContext.Set<Country>();
            if (await countryContext.AnyAsync(cancellationToken)) return;

            const string targetDir = "static";
            if (!DirectoryHelper.TryGetDirectoryInfo(targetDir, out var directory))
            {
                return;
            }

            string path = Path.Combine(directory!.FullName, targetDir, "json", "countries.json");
            var text = await File.ReadAllTextAsync(path, cancellationToken);
            var countries = JsonSerializer.Deserialize<List<Country>>(
                text, 
                options: new() 
                { 
                    PropertyNameCaseInsensitive = true,
                }
            )!;

            countries.RemoveAll(c => c.Cca2 == "CC" || c.Cca2 == "VA");

            await countryContext.AddRangeAsync(countries, cancellationToken);
        }
    }
}
#endregion