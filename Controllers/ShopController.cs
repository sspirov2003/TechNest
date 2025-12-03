using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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


    public IActionResult Index(string? query, int? categoryId)
    {
        var products = _context.Products.AsQueryable();


        if (categoryId.HasValue && categoryId > 0)
        {
            products = products.Where(p => p.CategoryId == categoryId);
        }


        if (!string.IsNullOrWhiteSpace(query))
        {
            query = query.ToLower();
            products = products.Where(p => p.Name.ToLower().Contains(query));
        }


        ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
        ViewBag.SelectedCategory = categoryId ?? 0;
        ViewBag.Query = query;

        return View(products.ToList());
    }


    [HttpGet]
    public JsonResult Search(string q, int? categoryId)
    {
        if (string.IsNullOrWhiteSpace(q))
            return Json(new List<string>());

        var query = _context.Products.AsQueryable();

        if (categoryId.HasValue && categoryId > 0)
            query = query.Where(p => p.CategoryId == categoryId);

        var matches = query
            .Where(p => p.Name.ToLower().Contains(q.ToLower()))
            .Select(p => p.Name)
            .Take(2)
            .ToList();

        return Json(matches);
    }
}
