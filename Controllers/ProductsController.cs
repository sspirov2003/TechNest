using Microsoft.AspNetCore.Mvc;
using TechNestClean.Data;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class ProductsController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: /Products
    public IActionResult Index()
    {
        var products = _context.Products.ToList();
        return View(products);
    }

    // GET: /Products/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (ModelState.IsValid)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(product);
    }

    // GET: /Products/Delete/5
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        return View(product);
    }

// POST: /Products/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

            _context.Products.Remove(product);
            _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

}

