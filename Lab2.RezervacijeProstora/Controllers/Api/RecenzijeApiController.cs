using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/recenzije")]
    public class RecenzijeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public RecenzijeApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecenzijaReadDto>>> GetAll([FromQuery] string? query)
        {
            var recenzijeQuery = BaseQuery();

            if (!string.IsNullOrWhiteSpace(query))
            {
                recenzijeQuery = recenzijeQuery.Where(r =>
                    r.Komentar.Contains(query) ||
                    r.Korisnik.ImePrezime.Contains(query) ||
                    r.Prostor.Naziv.Contains(query));
            }

            var recenzije = await recenzijeQuery
                .OrderByDescending(r => r.DatumRecenzije)
                .ToListAsync();

            return Ok(recenzije.Select(r => r.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<RecenzijaReadDto>> GetById(int id)
        {
            var recenzija = await BaseQuery().FirstOrDefaultAsync(r => r.Id == id);

            if (recenzija == null)
            {
                return NotFound();
            }

            return Ok(recenzija.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<RecenzijaReadDto>> Create([FromBody] RecenzijaCreateDto dto)
        {
            var validationError = await ValidateReferencesAsync(dto.KorisnikId, dto.ProstorId);
            if (validationError != null)
            {
                return validationError;
            }

            var recenzija = new Recenzija
            {
                Ocjena = dto.Ocjena,
                Komentar = dto.Komentar,
                DatumRecenzije = dto.DatumRecenzije,
                KorisnikId = dto.KorisnikId,
                ProstorId = dto.ProstorId
            };

            _context.Recenzije.Add(recenzija);
            await _context.SaveChangesAsync();

            var created = await BaseQuery().FirstAsync(r => r.Id == recenzija.Id);

            return CreatedAtAction(nameof(GetById), new { id = recenzija.Id }, created.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<RecenzijaReadDto>> Update(int id, [FromBody] RecenzijaUpdateDto dto)
        {
            var recenzija = await _context.Recenzije.FindAsync(id);

            if (recenzija == null)
            {
                return NotFound();
            }

            var validationError = await ValidateReferencesAsync(dto.KorisnikId, dto.ProstorId);
            if (validationError != null)
            {
                return validationError;
            }

            recenzija.Ocjena = dto.Ocjena;
            recenzija.Komentar = dto.Komentar;
            recenzija.DatumRecenzije = dto.DatumRecenzije;
            recenzija.KorisnikId = dto.KorisnikId;
            recenzija.ProstorId = dto.ProstorId;

            await _context.SaveChangesAsync();

            var updated = await BaseQuery().FirstAsync(r => r.Id == id);

            return Ok(updated.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var recenzija = await _context.Recenzije.FindAsync(id);

            if (recenzija == null)
            {
                return NotFound();
            }

            _context.Recenzije.Remove(recenzija);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private IQueryable<Recenzija> BaseQuery()
        {
            return _context.Recenzije
                .Include(r => r.Korisnik)
                .Include(r => r.Prostor);
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
