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


    public IActionResult Index()
    {
        var products = _context.Products.ToList();
        return View(products);
    }


    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = _context.Categories.ToList();
        return View();
    }


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


    [HttpGet]
    public IActionResult Delete(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        return View(product);
    }


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


    [HttpGet]
    public IActionResult Edit(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        ViewBag.Categories = _context.Categories.ToList();
        return View(product);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product product)
    {
        if (!ModelState.IsValid)
            return View(product);

        _context.Products.Update(product);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }


}

