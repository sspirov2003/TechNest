using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechNestClean.Data;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class CartController : Controller
{
    private readonly ApplicationDbContext _context;

    public CartController(ApplicationDbContext context)
    {
        _context = context;
    }

    private int? GetCurrentUserId()
    {
        return HttpContext.Session.GetInt32("CurrentUserId");
    }

    // GET: /Cart
    public IActionResult Index()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            TempData["CartMessage"] = "You must be logged in to view your cart.";
            return RedirectToAction("Login", "Auth");
        }

        var items = _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.AppUserId == userId.Value)
            .ToList();

        var total = items.Sum(i => (i.Product?.Price ?? 0m) * i.Quantity);
        ViewBag.Total = total;

        return View(items);
    }

    // POST: /Cart/Add/5
    [HttpPost]
    public IActionResult Add(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
        {
            TempData["CartMessage"] = "You must be logged in to add items to the cart.";
            return RedirectToAction("Login", "Auth");
        }

        var product = _context.Products.FirstOrDefault(p => p.Id == id);
        if (product == null)
            return NotFound();

        var existing = _context.CartItems
            .FirstOrDefault(ci => ci.AppUserId == userId.Value && ci.ProductId == id);

        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            _context.CartItems.Add(new CartItem
            {
                AppUserId = userId.Value,
                ProductId = id,
                Quantity = 1
            });
        }

        _context.SaveChanges();
        return RedirectToAction("Index");
    }

    // POST: /Cart/Remove/5   (5 = productId)
    [HttpPost]
    public IActionResult Remove(int id)
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var item = _context.CartItems
            .FirstOrDefault(ci => ci.AppUserId == userId.Value && ci.ProductId == id);

        if (item != null)
        {
            _context.CartItems.Remove(item);
            _context.SaveChanges();
        }

        return RedirectToAction("Index");
    }

    // POST: /Cart/Clear
    [HttpPost]
    public IActionResult Clear()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var items = _context.CartItems
            .Where(ci => ci.AppUserId == userId.Value);

        _context.CartItems.RemoveRange(items);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    // POST: /Cart/ApplyPromoCode  (keep your existing logic, just no GetCart/SaveCart needed)
    [HttpPost]
    public IActionResult ApplyPromoCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
        {
            TempData["PromoMessage"] = "Please enter a promo code.";
            return RedirectToAction("Index");
        }

        var promo = _context.PromoCodes
            .FirstOrDefault(p => p.Code.ToLower() == code.ToLower()
                                 && p.IsActive
                                 && p.ExpiryDate > DateTime.Now);

        if (promo == null)
        {
            TempData["PromoMessage"] = "Invalid or expired promo code.";
            return RedirectToAction("Index");
        }

        TempData["DiscountPercent"] = promo.DiscountPercent;
        TempData["PromoMessage"] = $"Promo code '{promo.Code}' applied! You get {promo.DiscountPercent}% off.";
        return RedirectToAction("Index");
    }

    // POST: /Cart/PlaceOrder
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult PlaceOrder()
    {
        var userId = GetCurrentUserId();
        if (userId == null)
            return RedirectToAction("Login", "Auth");

        var items = _context.CartItems
            .Include(ci => ci.Product)
            .Where(ci => ci.AppUserId == userId.Value)
            .ToList();

        if (!items.Any())
        {
            TempData["CartMessage"] = "Your cart is empty.";
            return RedirectToAction("Index");
        }

        var processingStatus = _context.OrderStatuses
            .FirstOrDefault(s => s.Name == "Processing")
            ?? _context.OrderStatuses.First();

        foreach (var ci in items)
        {
            var order = new Order
            {
                AppUserId = userId.Value,
                ProductId = ci.ProductId,
                OrderDate = DateTime.UtcNow,
                OrderStatusId = processingStatus.Id
            };
            _context.Orders.Add(order);
        }

        _context.SaveChanges();

        _context.CartItems.RemoveRange(items);
        _context.SaveChanges();

        TempData["CartMessage"] = "Order placed successfully!";
        return RedirectToAction("My", "Orders");
    }
}
