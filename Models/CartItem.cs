namespace TechNestClean.Models;

public class CartItem
{
    public int Id { get; set; }              // Primary key for EF

    public int AppUserId { get; set; }       // FK to AppUser
    public AppUser? AppUser { get; set; }

    public int ProductId { get; set; }       // FK to Product
    public Product? Product { get; set; }

    public int Quantity { get; set; } = 1;
}
