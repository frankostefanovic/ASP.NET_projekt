using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lab2.RezervacijeProstora.Controllers
{
    [Authorize]
    public class PlacanjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacanjeController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var placanja = await _context.Placanja
                .Include(p => p.Rezervacija)
                    .ThenInclude(r => r.Korisnik)
                .Include(p => p.Rezervacija)
                    .ThenInclude(r => r.Prostor)
                .OrderByDescending(p => p.DatumPlacanja)
                .ToListAsync();

            return View(placanja);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Placanja
                .Include(p => p.Rezervacija)
                    .ThenInclude(r => r.Korisnik)
                .Include(p => p.Rezervacija)
                    .ThenInclude(r => r.Prostor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var naciniPlacanja = Enum.GetValues<NacinPlacanja>()
                    .Where(n => n.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                query = query.Where(p =>
                    p.BrojTransakcije.Contains(q) ||
                    naciniPlacanja.Contains(p.NacinPlacanja) ||
                    p.Rezervacija.Korisnik.ImePrezime.Contains(q) ||
                    p.Rezervacija.Prostor.Naziv.Contains(q));
            }

            var placanja = await query
                .OrderByDescending(p => p.DatumPlacanja)
                .ToListAsync();

            return PartialView("_PlacanjeCards", placanja);
        }

        public async Task<IActionResult> Details(int id)
        {
            var placanje = await _context.Placanja
                .Include(p => p.Rezervacija)
                    .ThenInclude(r => r.Korisnik)
                .Include(p => p.Rezervacija)
                    .ThenInclude(r => r.Prostor)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (placanje == null)
            {
                return NotFound();
            }

            return View(placanje);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            await PopulateAutocompleteLabelsAsync();
            return View(new Placanje { DatumPlacanja = DateTime.Today, Uspjesno = true });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Iznos,DatumPlacanja,Uspjesno,NacinPlacanja,BrojTransakcije,RezervacijaId")] Placanje placanje)
        {
            RemoveNavigationValidation();

            if (ModelState.IsValid)
            {
                _context.Add(placanje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(placanje.RezervacijaId);
            return View(placanje);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Edit(int id)
        {
            var placanje = await _context.Placanja.FindAsync(id);

            if (placanje == null)
            {
                return NotFound();
            }

            await PopulateAutocompleteLabelsAsync(placanje.RezervacijaId);
            return View(placanje);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Iznos,DatumPlacanja,Uspjesno,NacinPlacanja,BrojTransakcije,RezervacijaId")] Placanje placanje)
        {
            RemoveNavigationValidation();

            if (id != placanje.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(placanje);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Placanja.AnyAsync(p => p.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(placanje.RezervacijaId);
            return View(placanje);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var placanje = await _context.Placanja
                .Include(p => p.Rezervacija)
                    .ThenInclude(r => r.Korisnik)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (placanje == null)
            {
                return NotFound();
            }

            return View(placanje);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var placanje = await _context.Placanja.FindAsync(id);

            if (placanje != null)
            {
                _context.Placanja.Remove(placanje);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        public async Task<IActionResult> AutocompleteRezervacije(string? term)
        {
            var query = _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                var hasIdTerm = int.TryParse(term, out var idTerm);

                query = query.Where(r =>
                    (hasIdTerm && r.Id == idTerm) ||
                    r.Korisnik.ImePrezime.Contains(term) ||
                    r.Prostor.Naziv.Contains(term));
            }

            var rezervacije = await query
                .OrderByDescending(r => r.DatumVrijemeOd)
                .Take(10)
                .Select(r => new { id = r.Id, label = r.Id + " - " + r.Korisnik.ImePrezime + " / " + r.Prostor.Naziv })
                .ToListAsync();

            return Json(rezervacije);
        }

        private async Task PopulateAutocompleteLabelsAsync(int? rezervacijaId = null)
        {
            ViewBag.RezervacijaLabel = rezervacijaId.HasValue
                ? await _context.Rezervacije
                    .Include(r => r.Korisnik)
                    .Include(r => r.Prostor)
                    .Where(r => r.Id == rezervacijaId)
                    .Select(r => r.Id + " - " + r.Korisnik.ImePrezime + " / " + r.Prostor.Naziv)
                    .FirstOrDefaultAsync()
                : string.Empty;
        }

        private void RemoveNavigationValidation()
        {
            ModelState.Remove(nameof(Placanje.Rezervacija));
        }
    }
}
