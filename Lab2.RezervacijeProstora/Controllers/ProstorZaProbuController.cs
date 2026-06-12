using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lab2.RezervacijeProstora.Controllers
{
    [Authorize]
    public class ProstorZaProbuController : Controller
    {
        private static readonly HashSet<string> AllowedUploadContentTypes = new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp",
            "application/pdf",
            "text/plain"
        };

        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProstorZaProbuController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [AllowAnonymous]
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

        [AllowAnonymous]
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

        [AllowAnonymous]
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
                .Include(p => p.Datoteke)
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

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Files(int id)
        {
            var datoteke = await _context.ProstorDatoteke
                .Where(d => d.ProstorZaProbuId == id)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            ViewBag.CanManageFiles = User.IsInRole("Admin") || User.IsInRole("Manager");
            return PartialView("_ProstorDatoteke", datoteke);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFiles(int id, List<IFormFile> files)
        {
            if (!await _context.Prostori.AnyAsync(p => p.Id == id))
            {
                return NotFound();
            }

            if (files.Count == 0)
            {
                return BadRequest("Odaberite barem jednu datoteku.");
            }

            var uploadRoot = Path.Combine(_environment.WebRootPath, "uploads", "prostori", id.ToString());
            Directory.CreateDirectory(uploadRoot);

            foreach (var file in files.Where(f => f.Length > 0))
            {
                if (!AllowedUploadContentTypes.Contains(file.ContentType))
                {
                    return BadRequest($"Tip datoteke nije podrzan: {file.FileName}");
                }

                var extension = Path.GetExtension(file.FileName);
                var storedName = $"{Guid.NewGuid():N}{extension}";
                var physicalPath = Path.Combine(uploadRoot, storedName);

                await using (var stream = System.IO.File.Create(physicalPath))
                {
                    await file.CopyToAsync(stream);
                }

                var relativePath = $"/uploads/prostori/{id}/{storedName}";
                _context.ProstorDatoteke.Add(new ProstorDatoteka
                {
                    ProstorZaProbuId = id,
                    FileName = Path.GetFileName(file.FileName),
                    FilePath = relativePath,
                    ContentType = file.ContentType,
                    FileSize = file.Length,
                    CreatedAt = DateTime.Now
                });
            }

            await _context.SaveChangesAsync();
            return await Files(id);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFile(int id)
        {
            var datoteka = await _context.ProstorDatoteke.FindAsync(id);

            if (datoteka == null)
            {
                return NotFound();
            }

            var prostorId = datoteka.ProstorZaProbuId;
            var relativePath = datoteka.FilePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var physicalPath = Path.Combine(_environment.WebRootPath, relativePath);

            _context.ProstorDatoteke.Remove(datoteka);
            await _context.SaveChangesAsync();

            if (System.IO.File.Exists(physicalPath))
            {
                System.IO.File.Delete(physicalPath);
            }

            return await Files(prostorId);
        }

        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> Create()
        {
            await PopulateAutocompleteLabelsAsync();
            return View(new ProstorZaProbu { Aktivan = true, DatumDodavanja = DateTime.Today });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
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

        [Authorize(Roles = "Admin,Manager")]
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
        [Authorize(Roles = "Admin,Manager")]
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

        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
            ModelState.Remove(nameof(ProstorZaProbu.Datoteke));
        }
    }
}
