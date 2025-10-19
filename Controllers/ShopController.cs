using Microsoft.AspNetCore.Mvc;
using TechNestClean.Data;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class ShopController : Controller
{
    private readonly ApplicationDbContext _context;

    public ShopController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string? query)
    {
        var products = _context.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            // Case-insensitive search
            query = query.ToLower();
            products = products.Where(p => p.Name.ToLower().Contains(query));
        }

        ViewBag.Query = query;
        return View(products.ToList());
    }

    [HttpGet]
    public JsonResult Search(string q)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Json(new List<string>());

        var matches = _context.Products
            .Where(p => p.Name.ToLower().Contains(q.ToLower()))
            .Select(p => p.Name)
            .Take(2)
            .ToList();

        return Json(matches);
    }
}
