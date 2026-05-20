using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class RezervacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RezervacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var rezervacije = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                    .ThenInclude(p => p.Lokacija)
                .Include(r => r.Placanje)
                .OrderByDescending(r => r.DatumVrijemeOd)
                .ToListAsync();

            return View(rezervacije);
        }

        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                    .ThenInclude(p => p.Lokacija)
                .Include(r => r.Placanje)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var statusi = Enum.GetValues<StatusRezervacije>()
                    .Where(s => s.ToString().Contains(q, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                query = query.Where(r =>
                    r.Korisnik.ImePrezime.Contains(q) ||
                    r.Prostor.Naziv.Contains(q) ||
                    r.Prostor.Lokacija.Grad.Contains(q) ||
                    statusi.Contains(r.Status) ||
                    r.Napomena.Contains(q));
            }

            var rezervacije = await query
                .OrderByDescending(r => r.DatumVrijemeOd)
                .ToListAsync();

            return PartialView("_RezervacijaCards", rezervacije);
        }

        public async Task<IActionResult> Details(int id)
        {
            var rezervacija = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                    .ThenInclude(p => p.Lokacija)
                .Include(r => r.Prostor)
                    .ThenInclude(p => p.Vlasnik)
                .Include(r => r.Placanje)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        public async Task<IActionResult> Create()
        {
            await PopulateAutocompleteLabelsAsync();
            return View(new Rezervacija
            {
                DatumVrijemeOd = DateTime.Now,
                DatumVrijemeDo = DateTime.Now.AddHours(2),
                DatumKreiranja = DateTime.Today,
                Status = StatusRezervacije.NaCekanju
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DatumVrijemeOd,DatumVrijemeDo,DatumKreiranja,Status,BrojSudionika,Napomena,KorisnikId,ProstorId")] Rezervacija rezervacija)
        {
            RemoveNavigationValidation();

            if (ModelState.IsValid)
            {
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(rezervacija.KorisnikId, rezervacija.ProstorId);
            return View(rezervacija);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            await PopulateAutocompleteLabelsAsync(rezervacija.KorisnikId, rezervacija.ProstorId);
            return View(rezervacija);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DatumVrijemeOd,DatumVrijemeDo,DatumKreiranja,Status,BrojSudionika,Napomena,KorisnikId,ProstorId")] Rezervacija rezervacija)
        {
            RemoveNavigationValidation();

            if (id != rezervacija.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Rezervacije.AnyAsync(r => r.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            await PopulateAutocompleteLabelsAsync(rezervacija.KorisnikId, rezervacija.ProstorId);
            return View(rezervacija);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var rezervacija = await _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
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
            ModelState.Remove(nameof(Rezervacija.Korisnik));
            ModelState.Remove(nameof(Rezervacija.Prostor));
            ModelState.Remove(nameof(Rezervacija.Placanje));
        }
    }
}
