using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class OpremaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OpremaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var oprema = await _context.Oprema
                .Include(o => o.Prostori)
                .OrderBy(o => o.Naziv)
                .ToListAsync();

            return View(oprema);
        }

        public async Task<IActionResult> Details(int id)
        {
            var oprema = await _context.Oprema
                .Include(o => o.Prostori)
                    .ThenInclude(p => p.Lokacija)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oprema == null)
            {
                return NotFound();
            }

            return View(oprema);
        }
    }
}
