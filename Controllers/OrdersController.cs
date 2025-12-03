using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechNestClean.Data;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class OrdersController : Controller
{
    private readonly ApplicationDbContext _context;

    public OrdersController(ApplicationDbContext context)
    {
        _context = context;
    }

    private UserRole? GetCurrentRole()
    {
        var roleString = HttpContext.Session.GetString("UserRole");
        if (!string.IsNullOrEmpty(roleString) &&
            Enum.TryParse<UserRole>(roleString, out var role))
        {
            return role;
        }
        return null;
    }

    private int? GetCurrentUserId()
    {
        return HttpContext.Session.GetInt32("CurrentUserId");
    }

    // USER: /Orders/My
    public IActionResult My()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var orders = _context.Orders
            .Include(o => o.Product)
            .Include(o => o.OrderStatus)
            .Where(o => o.AppUserId == userId.Value)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        return View(orders);
    }

    // MODERATOR/ADMIN: /Orders
    public IActionResult Index()
    {
        var role = GetCurrentRole();
        if (role != UserRole.Moderator && role != UserRole.Admin)
            return RedirectToAction("Index", "Home");

        var orders = _context.Orders
            .Include(o => o.Product)
            .Include(o => o.AppUser)
            .Include(o => o.OrderStatus)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        ViewBag.Statuses = _context.OrderStatuses.ToList();

        return View(orders);
    }

    // MODERATOR/ADMIN: update status
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult UpdateStatus(int id, int statusId)
    {
        var role = GetCurrentRole();
        if (role != UserRole.Moderator && role != UserRole.Admin)
            return RedirectToAction("Index", "Home");

        var order = _context.Orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
            return RedirectToAction(nameof(Index));

        order.OrderStatusId = statusId;
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}
