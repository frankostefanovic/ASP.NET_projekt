using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
