using Bogus;
using eCommerceWeb.Application.Abstractions.Database;
using eCommerceWeb.Domain.Entities.CatalogAggregate;
using eCommerceWeb.Domain.Entities.Misc;
using eCommerceWeb.Domain.Primitives.Storage;
using Microsoft.Extensions.Logging;
using SharedKernel.Utilities;

namespace eCommerceWeb.Persistence;

internal sealed class StoreDbContextInitializer(
    StoreDbContext storeDbContext,
    IFileStorageManger fileStorageManger,
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

    private async Task SeedProduct(CancellationToken cancellationToken)
    {
        if (!await storeDbContext.Set<Product>().AnyAsync(cancellationToken))
        {
            await fileStorageManger.ClearAsync(cancellationToken);

            var visibilities = Visibility.ListNames().Take(2);

            string[] names = [ 
                "Kids Sports Sneakers", "Mwv Sneakers", "Fasion Running Sneakers", 
                "Sport Running Sneakers", "YR7 SPY Sneakers", "Fasion Casual Sneakers", 
                "Yeezy Foam Runner", "New Balance Clog"
            ];

            (bool, decimal, decimal)[] price = [
                (false, 36.70m, 0m) ,
                (true ,23m, 15.69m),
                (false, 23m, 0m),
                (false, 23.50m, 0m),
                (true, 7.25m, 5m),
                (false, 21m, 0m),
                (false, 23m, 0m),
                (true, 50m, 37.50m)
            ];

            string[] images = [
                "https://www-konga-com-res.cloudinary.com/w_auto,f_auto,fl_lossy,dpr_auto,q_auto/media/catalog/product/I/I/68097_1668607420.jpg",
                "https://www-konga-com-res.cloudinary.com/w_400,f_auto,fl_lossy,dpr_3.0,q_auto/media/catalog/product/Q/O/199326_1694448366.jpg",
                "https://ng.jumia.is/unsafe/fit-in/500x500/filters:fill(white)/product/59/013819/2.jpg?5458",
                "https://ng.jumia.is/unsafe/fit-in/500x500/filters:fill(white)/product/63/142576/1.jpg?3347",
                "https://www-konga-com-res.cloudinary.com/w_400,f_auto,fl_lossy,dpr_3.0,q_auto/media/catalog/product/O/W/154509_1623809419.jpg",
                "https://ng.jumia.is/unsafe/fit-in/300x300/filters:fill(white)/product/67/4707162/1.jpg?5912",
                "https://hips.hearstapps.com/hmg-prod/images/index-shoes-1661358805.jpg?crop=0.502xw:1.00xh;0.250xw,0&resize=640:*",
                "https://hips.hearstapps.com/vader-prod.s3.amazonaws.com/1661357235-new-balance-clog-ivory-1661357229.jpg?crop=1xw:1xh;center,top&resize=980:*",
            ];

            for (int i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var(sale, unitP, saleP) = price[i];
                var isUnlimited = _faker.Random.Bool();
                int? quantity = isUnlimited ? null : _faker.Random.Int(25);
                var sku = $"SQ{_faker.Random.Int(1001111, 9919019)}";
                var desc = string.Join(Environment.NewLine, _faker.Lorem.Sentences(_faker.Random.Int(1, 5)));
                var model = new ProductCreationModel(
                    sku, name, desc, // details
                    sale, unitP, saleP, quantity, isUnlimited, // inventory
                    _faker.Random.Bool(), true, false, SeoHelper.GenerateUrlSlug(name), _faker.Lorem.Sentence(2),
                    null, null, null // seo
                );

                var product = new Product(model);
                product.SetVisibility(visibilities.RandomChoice());

                using var stream = new MemoryStream(await DownloadFromUrlAsync(images[i]));
                if (stream.Length != 0)
                {
                    var file = new MediaFile(stream.Length, "Image", $"IMG_{i:D4}.jpg", "image/jpg");
                    file.SetLocation(Path.Combine(file.FileType, $"{file.Id}"));
                    await storeDbContext.AddAsync(file, cancellationToken);

                    await fileStorageManger.UploadAsync(file, stream, cancellationToken);

                    product.AddImage(new()
                    { 
                        Uri = file.FileLocation, 
                        IsThumbnail = true, 
                        DisplayOrder = 1 
                    });
                }

                await storeDbContext.Set<Product>().AddAsync(product, cancellationToken);
            }

            async Task<byte[]> DownloadFromUrlAsync(string url)
            {
                using var client = new HttpClient();
                var uri = new Uri(url).GetLeftPart(UriPartial.Path);
                return await client.GetByteArrayAsync(uri, cancellationToken);
            }
        }
    }
}
