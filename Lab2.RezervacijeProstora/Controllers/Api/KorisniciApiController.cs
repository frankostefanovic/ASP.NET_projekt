using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/korisnici")]
    public class KorisniciApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public KorisniciApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KorisnikReadDto>>> GetAll([FromQuery] string? query)
        {
            var korisniciQuery = _context.Korisnici.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
            {
                korisniciQuery = korisniciQuery.Where(k =>
                    k.KorisnickoIme.Contains(query) ||
                    k.ImePrezime.Contains(query) ||
                    k.Email.Contains(query) ||
                    k.BrojTelefona.Contains(query));
            }

            var korisnici = await korisniciQuery
                .OrderBy(k => k.ImePrezime)
                .ToListAsync();

            return Ok(korisnici.Select(k => k.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<KorisnikReadDto>> GetById(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            return Ok(korisnik.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<KorisnikReadDto>> Create([FromBody] KorisnikCreateDto dto)
        {
            var korisnik = new Korisnik
            {
                KorisnickoIme = dto.KorisnickoIme,
                ImePrezime = dto.ImePrezime,
                Email = dto.Email,
                BrojTelefona = dto.BrojTelefona,
                DatumRegistracije = dto.DatumRegistracije,
                TipKorisnika = dto.TipKorisnika
            };

            _context.Korisnici.Add(korisnik);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = korisnik.Id }, korisnik.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<KorisnikReadDto>> Update(int id, [FromBody] KorisnikUpdateDto dto)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            korisnik.KorisnickoIme = dto.KorisnickoIme;
            korisnik.ImePrezime = dto.ImePrezime;
            korisnik.Email = dto.Email;
            korisnik.BrojTelefona = dto.BrojTelefona;
            korisnik.DatumRegistracije = dto.DatumRegistracije;
            korisnik.TipKorisnika = dto.TipKorisnika;

            await _context.SaveChangesAsync();

            return Ok(korisnik.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var korisnik = await _context.Korisnici.FindAsync(id);

            if (korisnik == null)
            {
                return NotFound();
            }

            _context.Korisnici.Remove(korisnik);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Korisnik se ne moze obrisati jer ima povezane zapise.");
            }

            return NoContent();
        }
    }
}
