using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class ProstorZaProbuReadDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public int KapacitetOsoba { get; set; }
        public decimal CijenaPoSatu { get; set; }
        public bool ImaParking { get; set; }
        public bool ImaKlimu { get; set; }
        public bool Aktivan { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public LokacijaReadDto? Lokacija { get; set; }
        public VlasnikReadDto? Vlasnik { get; set; }
        public List<OpremaReadDto> Oprema { get; set; } = new();
        public List<ProstorDatotekaReadDto> Datoteke { get; set; } = new();
    }

    public class ProstorZaProbuCreateDto
    {
        [Required]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        public int KapacitetOsoba { get; set; }
        public decimal CijenaPoSatu { get; set; }
        public bool ImaParking { get; set; }
        public bool ImaKlimu { get; set; }
        public bool Aktivan { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public int LokacijaId { get; set; }
        public int VlasnikId { get; set; }
        public List<int> OpremaIds { get; set; } = new();
    }

    public class ProstorZaProbuUpdateDto : ProstorZaProbuCreateDto
    {
    }
}
