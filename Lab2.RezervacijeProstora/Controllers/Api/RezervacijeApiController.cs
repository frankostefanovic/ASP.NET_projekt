using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/rezervacije")]
    public class RezervacijeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RezervacijeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RezervacijaReadDto>>> GetAll([FromQuery] string? query)
        {
            var rezervacijeQuery = BaseQuery();

            if (!string.IsNullOrWhiteSpace(query))
            {
                rezervacijeQuery = rezervacijeQuery.Where(r =>
                    r.Korisnik.ImePrezime.Contains(query) ||
                    r.Korisnik.KorisnickoIme.Contains(query) ||
                    r.Prostor.Naziv.Contains(query) ||
                    r.Napomena.Contains(query));
            }

            var rezervacije = await rezervacijeQuery
                .OrderByDescending(r => r.DatumVrijemeOd)
                .ToListAsync();

            return Ok(rezervacije.Select(r => r.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RezervacijaReadDto>> GetById(int id)
        {
            var rezervacija = await BaseQuery().FirstOrDefaultAsync(r => r.Id == id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            return Ok(rezervacija.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<RezervacijaReadDto>> Create([FromBody] RezervacijaCreateDto dto)
        {
            var validationError = await ValidateReferencesAsync(dto.KorisnikId, dto.ProstorId);
            if (validationError != null)
            {
                return validationError;
            }

            var rezervacija = new Rezervacija
            {
                DatumVrijemeOd = dto.DatumVrijemeOd,
                DatumVrijemeDo = dto.DatumVrijemeDo,
                DatumKreiranja = dto.DatumKreiranja,
                Status = dto.Status,
                BrojSudionika = dto.BrojSudionika,
                Napomena = dto.Napomena,
                KorisnikId = dto.KorisnikId,
                ProstorId = dto.ProstorId
            };

            _context.Rezervacije.Add(rezervacija);
            await _context.SaveChangesAsync();

            var created = await BaseQuery().FirstAsync(r => r.Id == rezervacija.Id);

            return CreatedAtAction(nameof(GetById), new { id = rezervacija.Id }, created.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<RezervacijaReadDto>> Update(int id, [FromBody] RezervacijaUpdateDto dto)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            var validationError = await ValidateReferencesAsync(dto.KorisnikId, dto.ProstorId);
            if (validationError != null)
            {
                return validationError;
            }

            rezervacija.DatumVrijemeOd = dto.DatumVrijemeOd;
            rezervacija.DatumVrijemeDo = dto.DatumVrijemeDo;
            rezervacija.DatumKreiranja = dto.DatumKreiranja;
            rezervacija.Status = dto.Status;
            rezervacija.BrojSudionika = dto.BrojSudionika;
            rezervacija.Napomena = dto.Napomena;
            rezervacija.KorisnikId = dto.KorisnikId;
            rezervacija.ProstorId = dto.ProstorId;

            await _context.SaveChangesAsync();

            var updated = await BaseQuery().FirstAsync(r => r.Id == id);

            return Ok(updated.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);

            if (rezervacija == null)
            {
                return NotFound();
            }

            _context.Rezervacije.Remove(rezervacija);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private IQueryable<Rezervacija> BaseQuery()
        {
            return _context.Rezervacije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor)
                .Include(r => r.Placanje);
        }

        private async Task<ActionResult?> ValidateReferencesAsync(int korisnikId, int prostorId)
        {
            if (!await _context.Korisnici.AnyAsync(k => k.Id == korisnikId))
            {
                return BadRequest("Korisnik ne postoji.");
            }

            if (!await _context.Prostori.AnyAsync(p => p.Id == prostorId))
            {
                return BadRequest("Prostor ne postoji.");
            }

            return null;
        }
    }
}
