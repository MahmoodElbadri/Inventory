using InventoryApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Configurations;

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

