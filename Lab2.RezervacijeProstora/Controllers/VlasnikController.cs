using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class VlasnikController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VlasnikController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var vlasnici = await _context.Vlasnici
                .Include(v => v.Prostori)
                .OrderBy(v => v.ImePrezime)
                .ToListAsync();

            return View(vlasnici);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vlasnik = await _context.Vlasnici
                .Include(v => v.Prostori)
                    .ThenInclude(p => p.Lokacija)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vlasnik == null)
            {
                return NotFound();
            }

            return View(vlasnik);
        }
    }
}
