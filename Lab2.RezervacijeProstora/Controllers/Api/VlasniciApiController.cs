using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/vlasnici")]
    public class VlasniciApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public VlasniciApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VlasnikReadDto>>> GetAll([FromQuery] string? query)
        {
            var vlasniciQuery = _context.Vlasnici.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                vlasniciQuery = vlasniciQuery.Where(v =>
                    v.ImePrezime.Contains(query) ||
                    v.Email.Contains(query) ||
                    v.BrojTelefona.Contains(query) ||
                    v.Oib.Contains(query));
            }

            var vlasnici = await vlasniciQuery
                .OrderBy(v => v.ImePrezime)
                .ToListAsync();

            return Ok(vlasnici.Select(v => v.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VlasnikReadDto>> GetById(int id)
        {
            var vlasnik = await _context.Vlasnici.FindAsync(id);

            if (vlasnik == null)
            {
                return NotFound();
            }

            return Ok(vlasnik.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<VlasnikReadDto>> Create([FromBody] VlasnikCreateDto dto)
        {
            var vlasnik = new Vlasnik
            {
                ImePrezime = dto.ImePrezime,
                Email = dto.Email,
                BrojTelefona = dto.BrojTelefona,
                DatumRegistracije = dto.DatumRegistracije,
                Oib = dto.Oib
            };

            _context.Vlasnici.Add(vlasnik);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = vlasnik.Id }, vlasnik.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<VlasnikReadDto>> Update(int id, [FromBody] VlasnikUpdateDto dto)
        {
            var vlasnik = await _context.Vlasnici.FindAsync(id);

            if (vlasnik == null)
            {
                return NotFound();
            }

            vlasnik.ImePrezime = dto.ImePrezime;
            vlasnik.Email = dto.Email;
            vlasnik.BrojTelefona = dto.BrojTelefona;
            vlasnik.DatumRegistracije = dto.DatumRegistracije;
            vlasnik.Oib = dto.Oib;

            await _context.SaveChangesAsync();

            return Ok(vlasnik.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vlasnik = await _context.Vlasnici.FindAsync(id);

            if (vlasnik == null)
            {
                return NotFound();
            }

            _context.Vlasnici.Remove(vlasnik);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Vlasnik se ne moze obrisati jer ima povezane prostore.");
            }

            return NoContent();
        }
    }
}
