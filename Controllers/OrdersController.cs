using Microsoft.AspNetCore.Mvc;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class OrdersController : Controller
{
    // Placeholder: empty list + banner for moderators
    public IActionResult Index()
    {
        ViewBag.RoleMessage = "Moderators will see all purchase history here.";
        var empty = Enumerable.Empty<Order>();
        return View(empty);
    }
}
