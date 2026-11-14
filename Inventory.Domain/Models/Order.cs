using InventoryApi.Models;

namespace InventoryApi.Models;

public class Order : BaseEntity
{
    public int UserId { get; set; }
    public DateTime OrderedAt { get; set; }
    public OrderStatus Status { get; set; }
    public decimal Total { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
