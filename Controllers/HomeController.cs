using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechNestClean.Data;
using TechNestClean.ViewModels;

namespace TechNestClean.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var vm = new HomeViewModel
        {
            Categories = _context.Categories
                .OrderBy(c => c.Name)
                .ToList(),

            FeaturedProducts = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsFeatured)
                .OrderByDescending(p => p.Id)
                .Take(6)
                .ToList()
        };

        return View(vm);
    }
}
