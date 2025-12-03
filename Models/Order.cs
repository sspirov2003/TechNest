namespace TechNestClean.Models;

public class Order
{
    public int Id { get; set; }

    public int AppUserId { get; set; }
    public AppUser? AppUser { get; set; }

    public int ProductId { get; set; }
    public Product? Product { get; set; }

    public DateTime OrderDate { get; set; }

    // Make this nullable to avoid migration pain for existing rows
    public int? OrderStatusId { get; set; }
    public OrderStatus? OrderStatus { get; set; }
}
