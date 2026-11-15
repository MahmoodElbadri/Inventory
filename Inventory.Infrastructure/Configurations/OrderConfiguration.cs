using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;

public class OrderConfiguration:IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(o => o.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(o => o.Total)
            .HasPrecision(18, 2);
    }

    
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.HasKey(oi => oi.Id);
        builder.HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(oi => oi.Quantity)
            .IsRequired();

        builder.Property(oi => oi.UnitPrice)
        .HasColumnType("decimal(10,2)")
            .IsRequired();
        
        builder.Property(oi => oi.UnitPrice)
            .HasPrecision(18, 2);


    }
}

public class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryTransaction>
{
    public void Configure(EntityTypeBuilder<InventoryTransaction> builder)
    {
        builder.HasKey(ii => ii.Id);
        builder.Property(ii => ii.ProductId)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(ii => ii.Type)
            .IsRequired();
        builder.Property(ii => ii.QuantityChanged)
            .IsRequired();
    }
}

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ProductName)
            .IsRequired();

        builder.Property(p => p.Description)
            .HasMaxLength(500);
        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);

        builder.Property(p=>p.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();
    }
}

