using Microsoft.AspNetCore.Mvc;
using TechNestClean.Data;
using TechNestClean.Models;
using TechNestClean.ViewModels;

namespace TechNestClean.Controllers;

public class AuthController : Controller
{
    private readonly ApplicationDbContext _context;

    public AuthController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Auth/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View(new RegisterViewModel());
    }

    // POST: /Auth/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        // Check if email already used
        if (_context.Users.Any(u => u.Email == model.Email))
        {
            ModelState.AddModelError("Email", "Email is already registered.");
            return View(model);
        }

        var user = new AppUser
        {
            Name = model.Name,
            Email = model.Email,
            Password = model.Password, // Plain text for demo only
            Role = UserRole.User       // All new users start as User
        };

        _context.Users.Add(user);
        _context.SaveChanges();

        // Log them in
        SetUserSession(user);

        return RedirectToAction("Index", "Home");
    }

    // GET: /Auth/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View(new LoginViewModel());
    }

    // POST: /Auth/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _context.Users
            .FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);

        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            return View(model);
        }

        SetUserSession(user);

        return RedirectToAction("Index", "Home");
    }

    // POST: /Auth/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    private void SetUserSession(AppUser user)
    {
        HttpContext.Session.SetInt32("CurrentUserId", user.Id);
        HttpContext.Session.SetString("CurrentUserName", user.Name);
        HttpContext.Session.SetString("UserRole", user.Role.ToString());
    }
}
