using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/oprema")]
    public class OpremaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OpremaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OpremaReadDto>>> GetAll([FromQuery] string? query)
        {
            var opremaQuery = _context.Oprema.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                opremaQuery = opremaQuery.Where(o =>
                    o.Naziv.Contains(query) ||
                    o.Proizvodac.Contains(query));
            }

            var oprema = await opremaQuery
                .OrderBy(o => o.Naziv)
                .ToListAsync();

            return Ok(oprema.Select(o => o.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<OpremaReadDto>> GetById(int id)
        {
            var oprema = await _context.Oprema.FindAsync(id);

            if (oprema == null)
            {
                return NotFound();
            }

            return Ok(oprema.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<OpremaReadDto>> Create([FromBody] OpremaCreateDto dto)
        {
            var oprema = new Oprema
            {
                Naziv = dto.Naziv,
                Proizvodac = dto.Proizvodac,
                Ispravna = dto.Ispravna,
                Vrijednost = dto.Vrijednost
            };

            _context.Oprema.Add(oprema);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = oprema.Id }, oprema.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<OpremaReadDto>> Update(int id, [FromBody] OpremaUpdateDto dto)
        {
            var oprema = await _context.Oprema.FindAsync(id);

            if (oprema == null)
            {
                return NotFound();
            }

            oprema.Naziv = dto.Naziv;
            oprema.Proizvodac = dto.Proizvodac;
            oprema.Ispravna = dto.Ispravna;
            oprema.Vrijednost = dto.Vrijednost;

            await _context.SaveChangesAsync();

            return Ok(oprema.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var oprema = await _context.Oprema.FindAsync(id);

            if (oprema == null)
            {
                return NotFound();
            }

            _context.Oprema.Remove(oprema);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
