using Microsoft.AspNetCore.Mvc;
using TechNestClean.Data;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class UsersController : Controller
{
    private readonly ApplicationDbContext _context;

    public UsersController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool IsAdmin()
    {
        var roleString = HttpContext.Session.GetString("UserRole") ?? "User";
        return Enum.TryParse<UserRole>(roleString, out var role) && role == UserRole.Admin;
    }


    public IActionResult Index()
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        var users = _context.Users.ToList();
        return View(users);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateRole(int id, UserRole role)
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home"); 

        var user = _context.Users.FirstOrDefault(u => u.Id == id);
        if (user == null)
        {
            TempData["UserRoleMessage"] = "User not found.";
            return RedirectToAction(nameof(Index));
        }

        user.Role = role;
        _context.SaveChanges();

        TempData["UserRoleMessage"] = $"Updated role for {user.Name} to {role}.";
        return RedirectToAction(nameof(Index));
    }
}
