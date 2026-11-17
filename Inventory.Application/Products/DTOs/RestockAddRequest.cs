namespace Inventory.Application.Products.DTOs;

public class RestockAddRequest
{
    public int ProductId { get; set; }
    public int  Quantity { get; set; }
}