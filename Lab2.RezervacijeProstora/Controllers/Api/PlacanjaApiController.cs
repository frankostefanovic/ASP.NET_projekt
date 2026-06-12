using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/placanja")]
    public class PlacanjaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlacanjaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlacanjeReadDto>>> GetAll([FromQuery] string? query)
        {
            var placanjaQuery = _context.Placanja.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                placanjaQuery = placanjaQuery.Where(p =>
                    p.BrojTransakcije.Contains(query) ||
                    p.NacinPlacanja.ToString().Contains(query));
            }

            var placanja = await placanjaQuery
                .OrderByDescending(p => p.DatumPlacanja)
                .ToListAsync();

            return Ok(placanja.Select(p => p.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PlacanjeReadDto>> GetById(int id)
        {
            var placanje = await _context.Placanja.FindAsync(id);

            if (placanje == null)
            {
                return NotFound();
            }

            return Ok(placanje.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<PlacanjeReadDto>> Create([FromBody] PlacanjeCreateDto dto)
        {
            var validationError = await ValidateRezervacijaAsync(dto.RezervacijaId);
            if (validationError != null)
            {
                return validationError;
            }

            if (await _context.Placanja.AnyAsync(p => p.RezervacijaId == dto.RezervacijaId))
            {
                return BadRequest("Rezervacija vec ima placanje.");
            }

            var placanje = new Placanje
            {
                Iznos = dto.Iznos,
                DatumPlacanja = dto.DatumPlacanja,
                Uspjesno = dto.Uspjesno,
                NacinPlacanja = dto.NacinPlacanja,
                BrojTransakcije = dto.BrojTransakcije,
                RezervacijaId = dto.RezervacijaId
            };

            _context.Placanja.Add(placanje);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = placanje.Id }, placanje.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PlacanjeReadDto>> Update(int id, [FromBody] PlacanjeUpdateDto dto)
        {
            var placanje = await _context.Placanja.FindAsync(id);

            if (placanje == null)
            {
                return NotFound();
            }

            var validationError = await ValidateRezervacijaAsync(dto.RezervacijaId);
            if (validationError != null)
            {
                return validationError;
            }

            if (await _context.Placanja.AnyAsync(p => p.Id != id && p.RezervacijaId == dto.RezervacijaId))
            {
                return BadRequest("Rezervacija vec ima drugo placanje.");
            }

            placanje.Iznos = dto.Iznos;
            placanje.DatumPlacanja = dto.DatumPlacanja;
            placanje.Uspjesno = dto.Uspjesno;
            placanje.NacinPlacanja = dto.NacinPlacanja;
            placanje.BrojTransakcije = dto.BrojTransakcije;
            placanje.RezervacijaId = dto.RezervacijaId;

            await _context.SaveChangesAsync();

            return Ok(placanje.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var placanje = await _context.Placanja.FindAsync(id);

            if (placanje == null)
            {
                return NotFound();
            }

            _context.Placanja.Remove(placanje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<ActionResult?> ValidateRezervacijaAsync(int rezervacijaId)
        {
            if (!await _context.Rezervacije.AnyAsync(r => r.Id == rezervacijaId))
            {
                return BadRequest("Rezervacija ne postoji.");
            }

            return null;
        }
    }
}
