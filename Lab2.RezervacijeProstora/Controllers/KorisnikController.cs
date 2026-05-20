using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class KorisnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public KorisnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var korisnici = await _context.Korisnici
                .Include(k => k.Rezervacije)
                .OrderBy(k => k.ImePrezime)
                .ToListAsync();

            return View(korisnici);
        }

        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Korisnici
                .Include(k => k.Rezervacije)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(k =>
                    k.KorisnickoIme.Contains(q) ||
                    k.ImePrezime.Contains(q) ||
                    k.Email.Contains(q) ||
                    k.BrojTelefona.Contains(q));
            }

            var korisnici = await query
                .OrderBy(k => k.ImePrezime)
                .ToListAsync();

            return PartialView("_KorisnikCards", korisnici);
        }

        public async Task<IActionResult> Autocomplete(string? term)
        {
            var query = _context.Korisnici.AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(k =>
                    k.ImePrezime.Contains(term) ||
                    k.KorisnickoIme.Contains(term) ||
                    k.Email.Contains(term));
            }

            var korisnici = await query
                .OrderBy(k => k.ImePrezime)
                .Take(10)
                .Select(k => new { id = k.Id, label = k.ImePrezime + " (" + k.KorisnickoIme + ")" })
                .ToListAsync();

            return Json(korisnici);
        }

        public async Task<IActionResult> Details(int id)
        {
            var korisnik = await _context.Korisnici
                .Include(k => k.Rezervacije)
                    .ThenInclude(r => r.Prostor)
                .Include(k => k.Recenzije)
                    .ThenInclude(r => r.Prostor)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        public IActionResult Create()
        {
            return View(new Korisnik { DatumRegistracije = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KorisnickoIme,ImePrezime,Email,BrojTelefona,DatumRegistracije,TipKorisnika")] Korisnik korisnik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(korisnik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(korisnik);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,KorisnickoIme,ImePrezime,Email,BrojTelefona,DatumRegistracije,TipKorisnika")] Korisnik korisnik)
        {
            if (id != korisnik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(korisnik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Korisnici.AnyAsync(k => k.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(korisnik);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var korisnik = await _context.Korisnici
                .Include(k => k.Rezervacije)
                .Include(k => k.Recenzije)
                .FirstOrDefaultAsync(k => k.Id == id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return View(korisnik);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);

            if (korisnik != null)
            {
                _context.Korisnici.Remove(korisnik);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
