using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class ProstorZaProbuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProstorZaProbuController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var prostori = await _context.Prostori
                .Include(p => p.Lokacija)
                .Include(p => p.Vlasnik)
                .Include(p => p.Recenzije)
                .OrderBy(p => p.Naziv)
                .ToListAsync();

            return View(prostori);
        }

        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Prostori
                .Include(p => p.Lokacija)
                .Include(p => p.Vlasnik)
                .Include(p => p.Recenzije)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(p =>
                    p.Naziv.Contains(q) ||
                    p.Lokacija.Grad.Contains(q) ||
                    p.Lokacija.Adresa.Contains(q) ||
                    p.Vlasnik.ImePrezime.Contains(q));
            }

            var prostori = await query
                .OrderBy(p => p.Naziv)
                .ToListAsync();

            return PartialView("_ProstorZaProbuCards", prostori);
        }

        public async Task<IActionResult> Autocomplete(string? term)
        {
            var query = _context.Prostori
                .Include(p => p.Lokacija)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(term))
            {
                query = query.Where(p =>
                    p.Naziv.Contains(term) ||
                    p.Lokacija.Grad.Contains(term) ||
                    p.Lokacija.Adresa.Contains(term));
            }

            var prostori = await query
                .OrderBy(p => p.Naziv)
                .Take(10)
                .Select(p => new { id = p.Id, label = p.Naziv + " - " + p.Lokacija.Grad })
                .ToListAsync();

            return Json(prostori);
        }

        public async Task<IActionResult> Details(int id)
        {
            var prostor = await _context.Prostori
                .Include(p => p.Lokacija)
                .Include(p => p.Vlasnik)
                .Include(p => p.Oprema)
                .Include(p => p.Recenzije)
                    .ThenInclude(r => r.Korisnik)
                .Include(p => p.Rezervacije)
                    .ThenInclude(r => r.Korisnik)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prostor == null)
            {
                return NotFound();
            }

            return View(prostor);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateAutocompleteLabelsAsync();
            return View(new ProstorZaProbu { Aktivan = true, DatumDodavanja = DateTime.Today });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,KapacitetOsoba,CijenaPoSatu,ImaParking,ImaKlimu,Aktivan,DatumDodavanja,LokacijaId,VlasnikId")] ProstorZaProbu prostor)
        {
            RemoveNavigationValidation();

            if (ModelState.IsValid)
            {
                _context.Add(prostor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(prostor.LokacijaId, prostor.VlasnikId);
            return View(prostor);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var prostor = await _context.Prostori.FindAsync(id);

            if (prostor == null)
            {
                return NotFound();
            }

            await PopulateAutocompleteLabelsAsync(prostor.LokacijaId, prostor.VlasnikId);
            return View(prostor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,KapacitetOsoba,CijenaPoSatu,ImaParking,ImaKlimu,Aktivan,DatumDodavanja,LokacijaId,VlasnikId")] ProstorZaProbu prostor)
        {
            RemoveNavigationValidation();

            if (id != prostor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prostor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Prostori.AnyAsync(p => p.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(prostor.LokacijaId, prostor.VlasnikId);
            return View(prostor);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var prostor = await _context.Prostori
                .Include(p => p.Lokacija)
                .Include(p => p.Vlasnik)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prostor == null)
            {
                return NotFound();
            }

            return View(prostor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prostor = await _context.Prostori.FindAsync(id);

            if (prostor != null)
            {
                _context.Prostori.Remove(prostor);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task PopulateAutocompleteLabelsAsync(int? lokacijaId = null, int? vlasnikId = null)
        {
            ViewBag.LokacijaLabel = lokacijaId.HasValue
                ? await _context.Lokacije
                    .Where(l => l.Id == lokacijaId)
                    .Select(l => l.Adresa + ", " + l.Grad)
                    .FirstOrDefaultAsync()
                : string.Empty;

            ViewBag.VlasnikLabel = vlasnikId.HasValue
                ? await _context.Vlasnici
                    .Where(v => v.Id == vlasnikId)
                    .Select(v => v.ImePrezime + " (" + v.Email + ")")
                    .FirstOrDefaultAsync()
                : string.Empty;
        }

        private void RemoveNavigationValidation()
        {
            ModelState.Remove(nameof(ProstorZaProbu.Lokacija));
            ModelState.Remove(nameof(ProstorZaProbu.Vlasnik));
            ModelState.Remove(nameof(ProstorZaProbu.Oprema));
            ModelState.Remove(nameof(ProstorZaProbu.Rezervacije));
            ModelState.Remove(nameof(ProstorZaProbu.Recenzije));
        }
    }
}
