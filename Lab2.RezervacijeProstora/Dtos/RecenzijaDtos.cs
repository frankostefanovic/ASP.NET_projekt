using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class RecenzijaReadDto
    {
        public int Id { get; set; }
        public int Ocjena { get; set; }
        public string Komentar { get; set; } = string.Empty;
        public DateTime DatumRecenzije { get; set; }
        public KorisnikReadDto? Korisnik { get; set; }
        public ProstorSummaryDto? Prostor { get; set; }
    }

    public class RecenzijaCreateDto
    {
        [Range(1, 5)]
        public int Ocjena { get; set; }

        [Required]
        [StringLength(500)]
        public string Komentar { get; set; } = string.Empty;

        public DateTime DatumRecenzije { get; set; }
        public int KorisnikId { get; set; }
        public int ProstorId { get; set; }
    }

    public class RecenzijaUpdateDto : RecenzijaCreateDto
    {
    }
}
