using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class LokacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LokacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lokacije = await _context.Lokacije
                .Include(l => l.Prostori)
                .OrderBy(l => l.Grad)
                .ToListAsync();

            return View(lokacije);
        }

        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Lokacije
                .Include(l => l.Prostori)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(l =>
                    l.Grad.Contains(q) ||
                    l.Adresa.Contains(q) ||
                    l.PostanskiBroj.Contains(q) ||
                    l.Drzava.Contains(q));
            }

            var lokacije = await query
                .OrderBy(l => l.Grad)
                .ToListAsync();

            return PartialView("_LokacijaCards", lokacije);
        }

        public async Task<IActionResult> Autocomplete(string? term)
        {
            var query = _context.Lokacije.AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(l =>
                    l.Grad.Contains(term) ||
                    l.Adresa.Contains(term) ||
                    l.PostanskiBroj.Contains(term));
            }

            var lokacije = await query
                .OrderBy(l => l.Grad)
                .ThenBy(l => l.Adresa)
                .Take(10)
                .Select(l => new { id = l.Id, label = l.Adresa + ", " + l.Grad })
                .ToListAsync();

            return Json(lokacije);
        }

        public async Task<IActionResult> Details(int id)
        {
            var lokacija = await _context.Lokacije
                .Include(l => l.Prostori)
                    .ThenInclude(p => p.Vlasnik)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return View(lokacija);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Grad,Adresa,PostanskiBroj,Drzava")] Lokacija lokacija)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lokacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(lokacija);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var lokacija = await _context.Lokacije.FindAsync(id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return View(lokacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Grad,Adresa,PostanskiBroj,Drzava")] Lokacija lokacija)
        {
            if (id != lokacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lokacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Lokacije.AnyAsync(l => l.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(lokacija);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var lokacija = await _context.Lokacije
                .Include(l => l.Prostori)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return View(lokacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var lokacija = await _context.Lokacije.FindAsync(id);

            if (lokacija != null)
            {
                _context.Lokacije.Remove(lokacija);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
