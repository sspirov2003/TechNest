using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TechNestClean.Data;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;
    private const string CartSessionKey = "Cart";

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Helper methods
    private List<CartItem> GetCart()
    {
        var cartJson = HttpContext.Session.GetString(CartSessionKey);
        return string.IsNullOrEmpty(cartJson)
            ? new List<CartItem>()
            : JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
    }

    private void SaveCart(List<CartItem> cart)
    {
        var cartJson = JsonSerializer.Serialize(cart);
        HttpContext.Session.SetString(CartSessionKey, cartJson);
    }

    // GET: /Cart
    public IActionResult Index()
    {
        var cart = GetCart();
        ViewBag.Total = cart.Sum(i => i.Price * i.Quantity);
        return View(cart);
    }

    // POST: /Cart/Add/5
    [HttpPost]
    public IActionResult Add(int id)
    {
        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        var cart = GetCart();
        var existing = cart.FirstOrDefault(i => i.ProductId == id);
        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            cart.Add(new CartItem
            {
                ProductId = product.Id,
                ProductName = product.Name,
                Price = product.Price
            });
        }

        SaveCart(cart);
        return RedirectToAction("Index");
    }

    // POST: /Cart/Remove/5
    [HttpPost]
    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        var item = cart.FirstOrDefault(i => i.ProductId == id);
        if (item != null)
        {
            cart.Remove(item);
            SaveCart(cart);
        }

        return RedirectToAction("Index");
    }

    // POST: /Cart/Clear
    [HttpPost]
    public IActionResult Clear()
    {
        SaveCart(new List<CartItem>());
        return RedirectToAction("Index");
    }
}
