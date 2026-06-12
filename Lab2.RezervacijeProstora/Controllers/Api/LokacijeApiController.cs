using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/lokacije")]
    public class LokacijeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LokacijeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LokacijaReadDto>>> GetAll([FromQuery] string? query)
        {
            var lokacijeQuery = _context.Lokacije.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                lokacijeQuery = lokacijeQuery.Where(l =>
                    l.Grad.Contains(query) ||
                    l.Adresa.Contains(query) ||
                    l.PostanskiBroj.Contains(query) ||
                    l.Drzava.Contains(query));
            }

            var lokacije = await lokacijeQuery
                .OrderBy(l => l.Grad)
                .ThenBy(l => l.Adresa)
                .ToListAsync();

            return Ok(lokacije.Select(l => l.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LokacijaReadDto>> GetById(int id)
        {
            var lokacija = await _context.Lokacije.FindAsync(id);

            if (lokacija == null)
            {
                return NotFound();
            }

            return Ok(lokacija.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<LokacijaReadDto>> Create([FromBody] LokacijaCreateDto dto)
        {
            var lokacija = new Lokacija
            {
                Grad = dto.Grad,
                Adresa = dto.Adresa,
                PostanskiBroj = dto.PostanskiBroj,
                Drzava = dto.Drzava
            };

            _context.Lokacije.Add(lokacija);
            await _context.SaveChangesAsync();

            var readDto = lokacija.ToReadDto();

            return CreatedAtAction(nameof(GetById), new { id = lokacija.Id }, readDto);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<LokacijaReadDto>> Update(int id, [FromBody] LokacijaUpdateDto dto)
        {
            var lokacija = await _context.Lokacije.FindAsync(id);

            if (lokacija == null)
            {
                return NotFound();
            }

            lokacija.Grad = dto.Grad;
            lokacija.Adresa = dto.Adresa;
            lokacija.PostanskiBroj = dto.PostanskiBroj;
            lokacija.Drzava = dto.Drzava;

            await _context.SaveChangesAsync();

            return Ok(lokacija.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var lokacija = await _context.Lokacije.FindAsync(id);

            if (lokacija == null)
            {
                return NotFound();
            }

            _context.Lokacije.Remove(lokacija);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
