using System.ComponentModel.DataAnnotations;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class KorisnikReadDto
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; } = string.Empty;
        public string ImePrezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BrojTelefona { get; set; } = string.Empty;
        public DateTime DatumRegistracije { get; set; }
        public TipKorisnika TipKorisnika { get; set; }
    }

    public class KorisnikCreateDto
    {
        [Required]
        [StringLength(50)]
        public string KorisnickoIme { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ImePrezime { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [StringLength(30)]
        public string BrojTelefona { get; set; } = string.Empty;

        public DateTime DatumRegistracije { get; set; }
        public TipKorisnika TipKorisnika { get; set; }
    }

    public class KorisnikUpdateDto : KorisnikCreateDto
    {
    }
}
