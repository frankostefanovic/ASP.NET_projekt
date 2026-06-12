using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class OpremaReadDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public string Proizvodac { get; set; } = string.Empty;
        public bool Ispravna { get; set; }
        public decimal Vrijednost { get; set; }
    }

    public class OpremaCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Proizvodac { get; set; } = string.Empty;

        public bool Ispravna { get; set; }
        public decimal Vrijednost { get; set; }
    }

    public class OpremaUpdateDto : OpremaCreateDto
    {
    }
}
