using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    }
}
