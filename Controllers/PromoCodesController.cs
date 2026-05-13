using Microsoft.AspNetCore.Mvc;
using TechNestClean.Data;
using TechNestClean.Models;

namespace TechNestClean.Controllers;

public class PromoCodesController : Controller
{
    private readonly ApplicationDbContext _context;

    public PromoCodesController(ApplicationDbContext context)
    {
        _context = context;
    }

    private bool IsAdmin()
    {
        var roleString = HttpContext.Session.GetString("UserRole") ?? "User";
        return Enum.TryParse<UserRole>(roleString, out var role) && role == UserRole.Admin;
    }

    // GET: /PromoCodes
    public IActionResult Index()
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        var codes = _context.PromoCodes
            .OrderByDescending(p => p.IsActive)
            .ThenBy(p => p.ExpiryDate)
            .ToList();

        return View(codes);
    }

    // GET: /PromoCodes/Create
    [HttpGet]
    public IActionResult Create()
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        var model = new PromoCode
        {
            ExpiryDate = DateTime.Today.AddMonths(1),
            IsActive = true
        };

        return View(model);
    }

    // POST: /PromoCodes/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(PromoCode promo)
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        if (!ModelState.IsValid)
            return View(promo);

        _context.PromoCodes.Add(promo);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // GET: /PromoCodes/Edit/5
    [HttpGet]
    public IActionResult Edit(int id)
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        var promo = _context.PromoCodes.FirstOrDefault(p => p.Id == id);
        if (promo == null)
            return NotFound();

        return View(promo);
    }

    // POST: /PromoCodes/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, PromoCode promo)
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        if (id != promo.Id)
            return BadRequest();

        if (!ModelState.IsValid)
            return View(promo);

        _context.PromoCodes.Update(promo);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // POST: /PromoCodes/ToggleActive/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ToggleActive(int id)
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        var promo = _context.PromoCodes.FirstOrDefault(p => p.Id == id);
        if (promo == null)
            return NotFound();

        promo.IsActive = !promo.IsActive;
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    // POST: /PromoCodes/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        if (!IsAdmin())
            return RedirectToAction("Index", "Home");

        var promo = _context.PromoCodes.FirstOrDefault(p => p.Id == id);
        if (promo != null)
        {
            _context.PromoCodes.Remove(promo);
            _context.SaveChanges();
        }

        return RedirectToAction(nameof(Index));
    }
}
