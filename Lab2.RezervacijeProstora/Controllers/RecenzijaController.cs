using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class RecenzijaController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecenzijaController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var recenzije = await _context.Recenzije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .OrderByDescending(r => r.DatumRecenzije)
                .ToListAsync();

            return View(recenzije);
        }

        public async Task<IActionResult> Details(int id)
        {
            var recenzija = await _context.Recenzije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recenzija == null)
            {
                return NotFound();
            }

            return View(recenzija);
        }
    }
}
