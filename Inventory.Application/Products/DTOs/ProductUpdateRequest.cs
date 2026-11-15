namespace Inventory.Application.Products.DTOs;

public class ProductUpdateRequest
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityOnHand { get; set; }
}
