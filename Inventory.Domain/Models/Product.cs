namespace InventoryApi.Models;

public class Product : BaseEntity
{
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityOnHand { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
