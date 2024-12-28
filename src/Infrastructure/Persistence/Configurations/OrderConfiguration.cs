using System.Security.Cryptography;
using eCommerceWeb.Domain.Entities.CustomerAggregate;
using eCommerceWeb.Domain.Entities.OrderAggregate;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using SharedKernel.Constants;

namespace eCommerceWeb.Persistence.Configurations;

internal sealed class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.Property(o => o.Id).ValueGeneratedNever();

        builder.HasIndex(o => o.OrderNo).IsUnique();
        builder.Property(o => o.OrderNo)
            .HasMaxLength(DomainModelConstants.ORDER_TRACKING_NO_LENGTH)
            .IsFixedLength()
            .HasValueGenerator<OrderNumberGenerator>()
            .ValueGeneratedOnAdd();
        
        builder.HasIndex(o => o.OrderStatus);
        builder.HasIndex(o => o.PaymentStatus);

        builder.ComplexProperty(o => o.BillingAddress, ComplexObjectConfiguration.ConfigureAddress);
        builder.ComplexProperty(o => o.DeliveryAddress, ComplexObjectConfiguration.ConfigureAddress);

        /***** Relationships *****/

        builder.HasMany(o => o.OrderItems)
            .WithOne();

        builder.HasOne<Customer>()
            .WithMany()
            .HasForeignKey(o => o.CustomerId);

        /***** Table Configurations *****/

        builder.ToTable(b => 
        {
            b.HasCheckConstraint("ck_order_status", $"order_status IN ('{string.Join("', ", OrderStatus.ListNames())}')");
            b.HasCheckConstraint("ck_payment_status", $"payment_status IN ('{string.Join("', ", PaymentStatus.ListNames())}')");
        });
    }
}

internal sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);

        builder.ComplexProperty(oi => oi.ProductOrdered, b => 
        {
            b.IsRequired();
        });

        builder.ToTable(b => 
        {
            b.HasCheckConstraint("ck_quantity", "quantity > 0");
        });
    }
}

internal sealed class OrderNumberGenerator : ValueGenerator<string>
{
    private static readonly ThreadLocal<RandomNumberGenerator> _rng  = new(RandomNumberGenerator.Create);
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    public override bool GeneratesTemporaryValues { get; } = false;

    public override string Next(EntityEntry entry)
    {
        if (entry.Entity is not Order)
            throw new InvalidOperationException();

        return $"#{GenerateOrderNo()}";
    }

    private static string GenerateOrderNo()
    {
        var bytes = new byte[DomainModelConstants.ORDER_TRACKING_NO_LENGTH - 1];
        _rng.Value!.GetBytes(bytes);
        return new string([.. bytes.Select(b => chars[b % chars.Length])]);
    }
}
