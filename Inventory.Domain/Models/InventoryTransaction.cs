namespace InventoryApi.Models;

public class InventoryTransaction : BaseEntity
{
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int QuantityChanged { get; set; }
    public TransactionType Type { get; set; }
    public int? OrderId { get; set; }
    public Order? Order { get; set; }
    public string? AdjustmentReason { get; set; }
}
