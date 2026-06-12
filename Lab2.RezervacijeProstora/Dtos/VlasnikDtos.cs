using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class VlasnikReadDto
    {
        public int Id { get; set; }
        public string ImePrezime { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BrojTelefona { get; set; } = string.Empty;
        public DateTime DatumRegistracije { get; set; }
        public string Oib { get; set; } = string.Empty;
    }

    public class VlasnikCreateDto
    {
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

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string Oib { get; set; } = string.Empty;
    }

    public class VlasnikUpdateDto : VlasnikCreateDto
    {
    }
}
