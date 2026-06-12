using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class LokacijaReadDto
    {
        public int Id { get; set; }
        public string Grad { get; set; } = string.Empty;
        public string Adresa { get; set; } = string.Empty;
        public string PostanskiBroj { get; set; } = string.Empty;
        public string Drzava { get; set; } = string.Empty;
    }

    public class LokacijaCreateDto
    {
        [Required]
        [StringLength(80)]
        public string Grad { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Adresa { get; set; } = string.Empty;

        [Required]
        [StringLength(12)]
        public string PostanskiBroj { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Drzava { get; set; } = string.Empty;
    }

    public class LokacijaUpdateDto : LokacijaCreateDto
    {
    }
}
