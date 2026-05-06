using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
