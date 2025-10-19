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
    public string Name { get; set; } = default!;
    public UserRole Role { get; set; } = UserRole.User;
}
