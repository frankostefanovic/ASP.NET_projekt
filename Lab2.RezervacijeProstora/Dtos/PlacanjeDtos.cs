using System.ComponentModel.DataAnnotations;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class PlacanjeReadDto
    {
        public int Id { get; set; }
        public decimal Iznos { get; set; }
        public DateTime DatumPlacanja { get; set; }
        public bool Uspjesno { get; set; }
        public NacinPlacanja NacinPlacanja { get; set; }
        public string BrojTransakcije { get; set; } = string.Empty;
        public int RezervacijaId { get; set; }
    }

    public class PlacanjeCreateDto
    {
        public decimal Iznos { get; set; }
        public DateTime DatumPlacanja { get; set; }
        public bool Uspjesno { get; set; }
        public NacinPlacanja NacinPlacanja { get; set; }

        [StringLength(80)]
        public string BrojTransakcije { get; set; } = string.Empty;

        public int RezervacijaId { get; set; }
    }

    public class PlacanjeUpdateDto : PlacanjeCreateDto
    {
    }
}
