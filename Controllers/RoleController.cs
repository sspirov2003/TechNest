using Microsoft.AspNetCore.Mvc;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class RoleController : Controller
{
    public IActionResult Set(string role)
    {
        if (Enum.TryParse<UserRole>(role, out var parsedRole))
        {
            HttpContext.Session.SetString("UserRole", parsedRole.ToString());
        }

        // ✅ Redirect somewhere that definitely exists:
        return RedirectToAction("Index", "Products");
    }
}
