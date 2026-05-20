using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lab2.RezervacijeProstora.Models;

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

        public async Task<IActionResult> Search(string? q)
        {
            var query = _context.Oprema
                .Include(o => o.Prostori)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(o =>
                    o.Naziv.Contains(q) ||
                    o.Proizvodac.Contains(q));
            }

            var oprema = await query
                .OrderBy(o => o.Naziv)
                .ToListAsync();

            return PartialView("_OpremaCards", oprema);
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

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naziv,Proizvodac,Ispravna,Vrijednost")] Oprema oprema)
        {
            if (ModelState.IsValid)
            {
                _context.Add(oprema);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(oprema);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var oprema = await _context.Oprema.FindAsync(id);

            if (oprema == null)
            {
                return NotFound();
            }

            return View(oprema);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Naziv,Proizvodac,Ispravna,Vrijednost")] Oprema oprema)
        {
            if (id != oprema.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(oprema);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Oprema.AnyAsync(o => o.Id == id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return View(oprema);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var oprema = await _context.Oprema
                .Include(o => o.Prostori)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (oprema == null)
            {
                return NotFound();
            }

            return View(oprema);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var oprema = await _context.Oprema.FindAsync(id);

            if (oprema != null)
            {
                _context.Oprema.Remove(oprema);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
