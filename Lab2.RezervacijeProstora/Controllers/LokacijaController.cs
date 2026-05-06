using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class LokacijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LokacijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var lokacije = await _context.Lokacije
                .Include(l => l.Prostori)
                .OrderBy(l => l.Grad)
                .ToListAsync();

            return View(lokacije);
        }

        public async Task<IActionResult> Details(int id)
        {
            var lokacija = await _context.Lokacije
                .Include(l => l.Prostori)
                    .ThenInclude(p => p.Vlasnik)
                .FirstOrDefaultAsync(l => l.Id == id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return View(lokacija);
        }
    }
}
