using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class PlacanjeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlacanjeController(ApplicationDbContext context)
        {
            _context = context;
        }

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
    }
}
