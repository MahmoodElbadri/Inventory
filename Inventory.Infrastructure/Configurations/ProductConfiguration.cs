using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.ProductName)
            .IsRequired()
            .HasMaxLength(100);

        // 2. Unique Index Configuration
        builder.HasIndex(p => p.ProductName)
            .IsUnique()
            .HasFilter("[isDeleted] = 0");
       

        builder.Property(p => p.Description)
            .HasMaxLength(500);
        builder.Property(p => p.Price)
            .HasColumnType("decimal(10,2)")
            .IsRequired();

        builder.Property(p => p.Price)
            .HasPrecision(18, 2);


        builder.Property(p => p.RowVersion)
            .IsRowVersion()
            .IsConcurrencyToken();

    }
}

