using Lab2.RezervacijeProstora.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = new HomeIndexViewModel
            {
                BrojKorisnika = await _context.Korisnici.CountAsync(),
                BrojProstora = await _context.Prostori.CountAsync(),
                BrojRezervacija = await _context.Rezervacije.CountAsync(),
                BrojOpreme = await _context.Oprema.CountAsync(),

                IstaknutiProstori = await _context.Prostori
                    .Include(p => p.Lokacija)
                    .OrderBy(p => p.Id)
                    .Take(3)
                    .ToListAsync(),

                ZadnjeRezervacije = await _context.Rezervacije
                    .Include(r => r.Korisnik)
                    .Include(r => r.Prostor)
                    .OrderByDescending(r => r.DatumKreiranja)
                    .Take(3)
                    .ToListAsync()
            };

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
