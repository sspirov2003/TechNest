using Microsoft.AspNetCore.Mvc;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class UsersController : Controller
{
    // Static roles list; no DB yet on purpose
    public IActionResult Index()
    {
        var roles = Enum.GetNames(typeof(UserRole));
        return View(roles);
    }
}
