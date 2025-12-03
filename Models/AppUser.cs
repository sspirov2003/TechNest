namespace TechNestClean.Models;

 public enum UserRole
{
    Admin = 0,
    Moderator = 1,
    User = 2
}
 

public class AppUser
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    // NOTE: Plain-text for demo only. In real apps use hashing.
    public string Password { get; set; } = string.Empty;

    public UserRole Role { get; set; }
}
