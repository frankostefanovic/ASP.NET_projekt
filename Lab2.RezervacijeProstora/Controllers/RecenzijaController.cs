using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lab2.RezervacijeProstora.Controllers
{
    [Authorize]
    public class RecenzijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecenzijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var recenzije = await _context.Recenzije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .OrderByDescending(r => r.DatumRecenzije)
                .ToListAsync();

            return View(recenzije);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Recenzije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(r =>
                    r.Korisnik.ImePrezime.Contains(q) ||
                    r.Prostor.Naziv.Contains(q) ||
                    r.Komentar.Contains(q));
            }

            var recenzije = await query
                .OrderByDescending(r => r.DatumRecenzije)
                .ToListAsync();

            return PartialView("_RecenzijaCards", recenzije);
        }

        public async Task<IActionResult> Details(int id)
        {
            var recenzija = await _context.Recenzije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            await PopulateAutocompleteLabelsAsync();
            return View(new Recenzija { DatumRecenzije = DateTime.Today, Ocjena = 5 });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ocjena,Komentar,DatumRecenzije,KorisnikId,ProstorId")] Recenzija recenzija)
        {
            RemoveNavigationValidation();

            if (ModelState.IsValid)
            {
                _context.Add(recenzija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(recenzija.KorisnikId, recenzija.ProstorId);
            return View(recenzija);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var recenzija = await _context.Recenzije.FindAsync(id);

            if (recenzija == null)
            {
                return NotFound();
            }

            await PopulateAutocompleteLabelsAsync(recenzija.KorisnikId, recenzija.ProstorId);
            return View(recenzija);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ocjena,Komentar,DatumRecenzije,KorisnikId,ProstorId")] Recenzija recenzija)
        {
            RemoveNavigationValidation();

            if (id != recenzija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recenzija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Recenzije.AnyAsync(r => r.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(recenzija.KorisnikId, recenzija.ProstorId);
            return View(recenzija);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var recenzija = await _context.Recenzije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var recenzija = await _context.Recenzije.FindAsync(id);

            if (recenzija != null)
            {
                _context.Recenzije.Remove(recenzija);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAutocompleteLabelsAsync(int? korisnikId = null, int? prostorId = null)
        {
            ViewBag.KorisnikLabel = korisnikId.HasValue
                ? await _context.Korisnici
                    .Where(k => k.Id == korisnikId)
                    .Select(k => k.ImePrezime + " (" + k.KorisnickoIme + ")")
                    .FirstOrDefaultAsync()
                : string.Empty;

            ViewBag.ProstorLabel = prostorId.HasValue
                ? await _context.Prostori
                    .Include(p => p.Lokacija)
                    .Where(p => p.Id == prostorId)
                    .Select(p => p.Naziv + " - " + p.Lokacija.Grad)
                    .FirstOrDefaultAsync()
                : string.Empty;
        }

        private void RemoveNavigationValidation()
        {
            ModelState.Remove(nameof(Recenzija.Korisnik));
            ModelState.Remove(nameof(Recenzija.Prostor));
        }
    }
}
