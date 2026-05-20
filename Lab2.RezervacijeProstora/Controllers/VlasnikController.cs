using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class VlasnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VlasnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vlasnici = await _context.Vlasnici
                .Include(v => v.Prostori)
                .OrderBy(v => v.ImePrezime)
                .ToListAsync();

            return View(vlasnici);
        }

        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Vlasnici
                .Include(v => v.Prostori)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(v =>
                    v.ImePrezime.Contains(q) ||
                    v.Email.Contains(q) ||
                    v.BrojTelefona.Contains(q) ||
                    v.Oib.Contains(q));
            }

            var vlasnici = await query
                .OrderBy(v => v.ImePrezime)
                .ToListAsync();

            return PartialView("_VlasnikCards", vlasnici);
        }

        public async Task<IActionResult> Autocomplete(string? term)
        {
            var query = _context.Vlasnici.AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(v =>
                    v.ImePrezime.Contains(term) ||
                    v.Email.Contains(term) ||
                    v.Oib.Contains(term));
            }

            var vlasnici = await query
                .OrderBy(v => v.ImePrezime)
                .Take(10)
                .Select(v => new { id = v.Id, label = v.ImePrezime + " (" + v.Email + ")" })
                .ToListAsync();

            return Json(vlasnici);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vlasnik = await _context.Vlasnici
                .Include(v => v.Prostori)
                    .ThenInclude(p => p.Lokacija)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vlasnik == null)
            {
                return NotFound();
            }

            return View(vlasnik);
        }

        public IActionResult Create()
        {
            return View(new Vlasnik { DatumRegistracije = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ImePrezime,Email,BrojTelefona,DatumRegistracije,Oib")] Vlasnik vlasnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(vlasnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(vlasnik);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var vlasnik = await _context.Vlasnici.FindAsync(id);

            if (vlasnik == null)
            {
                return NotFound();
            }

            return View(vlasnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImePrezime,Email,BrojTelefona,DatumRegistracije,Oib")] Vlasnik vlasnik)
        {
            if (id != vlasnik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vlasnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Vlasnici.AnyAsync(v => v.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(vlasnik);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var vlasnik = await _context.Vlasnici
                .Include(v => v.Prostori)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vlasnik == null)
            {
                return NotFound();
            }

            return View(vlasnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var vlasnik = await _context.Vlasnici.FindAsync(id);

            if (vlasnik != null)
            {
                _context.Vlasnici.Remove(vlasnik);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
