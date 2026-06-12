using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Controllers.Api
{
    [ApiController]
    [Route("api/prostori")]
    public class ProstoriApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ProstoriApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProstorZaProbuReadDto>>> GetAll([FromQuery] string? query)
        {
            var prostoriQuery = BaseQuery();

            if (!string.IsNullOrWhiteSpace(query))
            {
                prostoriQuery = prostoriQuery.Where(p =>
                    p.Naziv.Contains(query) ||
                    p.Lokacija.Grad.Contains(query) ||
                    p.Lokacija.Adresa.Contains(query) ||
                    p.Vlasnik.ImePrezime.Contains(query));
            }

            var prostori = await prostoriQuery
                .OrderBy(p => p.Naziv)
                .ToListAsync();

            return Ok(prostori.Select(p => p.ToReadDto()));
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProstorZaProbuReadDto>> GetById(int id)
        {
            var prostor = await BaseQuery().FirstOrDefaultAsync(p => p.Id == id);

            if (prostor == null)
            {
                return NotFound();
            }

            return Ok(prostor.ToReadDto());
        }

        [HttpPost]
        public async Task<ActionResult<ProstorZaProbuReadDto>> Create([FromBody] ProstorZaProbuCreateDto dto)
        {
            var validationError = await ValidateReferencesAsync(dto.LokacijaId, dto.VlasnikId, dto.OpremaIds);
            if (validationError != null)
            {
                return validationError;
            }

            var prostor = new ProstorZaProbu
            {
                Naziv = dto.Naziv,
                KapacitetOsoba = dto.KapacitetOsoba,
                CijenaPoSatu = dto.CijenaPoSatu,
                ImaParking = dto.ImaParking,
                ImaKlimu = dto.ImaKlimu,
                Aktivan = dto.Aktivan,
                DatumDodavanja = dto.DatumDodavanja,
                LokacijaId = dto.LokacijaId,
                VlasnikId = dto.VlasnikId
            };

            await SetOpremaAsync(prostor, dto.OpremaIds);

            _context.Prostori.Add(prostor);
            await _context.SaveChangesAsync();

            var created = await BaseQuery().FirstAsync(p => p.Id == prostor.Id);

            return CreatedAtAction(nameof(GetById), new { id = prostor.Id }, created.ToReadDto());
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<ProstorZaProbuReadDto>> Update(int id, [FromBody] ProstorZaProbuUpdateDto dto)
        {
            var prostor = await _context.Prostori
                .Include(p => p.Oprema)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prostor == null)
            {
                return NotFound();
            }

            var validationError = await ValidateReferencesAsync(dto.LokacijaId, dto.VlasnikId, dto.OpremaIds);
            if (validationError != null)
            {
                return validationError;
            }

            prostor.Naziv = dto.Naziv;
            prostor.KapacitetOsoba = dto.KapacitetOsoba;
            prostor.CijenaPoSatu = dto.CijenaPoSatu;
            prostor.ImaParking = dto.ImaParking;
            prostor.ImaKlimu = dto.ImaKlimu;
            prostor.Aktivan = dto.Aktivan;
            prostor.DatumDodavanja = dto.DatumDodavanja;
            prostor.LokacijaId = dto.LokacijaId;
            prostor.VlasnikId = dto.VlasnikId;
            await SetOpremaAsync(prostor, dto.OpremaIds);

            await _context.SaveChangesAsync();

            var updated = await BaseQuery().FirstAsync(p => p.Id == id);

            return Ok(updated.ToReadDto());
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var prostor = await _context.Prostori.FindAsync(id);

            if (prostor == null)
            {
                return NotFound();
            }

            _context.Prostori.Remove(prostor);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Prostor se ne moze obrisati jer ima povezane rezervacije.");
            }

            return NoContent();
        }

        private IQueryable<ProstorZaProbu> BaseQuery()
        {
            return _context.Prostori
                .Include(p => p.Lokacija)
                .Include(p => p.Vlasnik)
                .Include(p => p.Oprema)
                .Include(p => p.Datoteke);
        }

        private async Task<ActionResult?> ValidateReferencesAsync(int lokacijaId, int vlasnikId, List<int> opremaIds)
        {
            if (!await _context.Lokacije.AnyAsync(l => l.Id == lokacijaId))
            {
                return BadRequest("Lokacija ne postoji.");
            }

            if (!await _context.Vlasnici.AnyAsync(v => v.Id == vlasnikId))
            {
                return BadRequest("Vlasnik ne postoji.");
            }

            var distinctOpremaIds = opremaIds.Distinct().ToList();
            var existingOpremaCount = await _context.Oprema.CountAsync(o => distinctOpremaIds.Contains(o.Id));

            if (existingOpremaCount != distinctOpremaIds.Count)
            {
                return BadRequest("Jedna ili vise stavki opreme ne postoji.");
            }

            return null;
        }

        private async Task SetOpremaAsync(ProstorZaProbu prostor, List<int> opremaIds)
        {
            prostor.Oprema.Clear();

            if (opremaIds.Count == 0)
            {
                return;
            }

            var oprema = await _context.Oprema
                .Where(o => opremaIds.Distinct().Contains(o.Id))
                .ToListAsync();

            foreach (var item in oprema)
            {
                prostor.Oprema.Add(item);
            }
        }
    }
}
