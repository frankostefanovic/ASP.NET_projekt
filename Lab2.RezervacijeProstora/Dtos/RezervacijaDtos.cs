using System.ComponentModel.DataAnnotations;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Dtos
{
    public class RezervacijaReadDto
    {
        public int Id { get; set; }
        public DateTime DatumVrijemeOd { get; set; }
        public DateTime DatumVrijemeDo { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public StatusRezervacije Status { get; set; }
        public int BrojSudionika { get; set; }
        public string Napomena { get; set; } = string.Empty;
        public KorisnikReadDto? Korisnik { get; set; }
        public ProstorSummaryDto? Prostor { get; set; }
        public PlacanjeReadDto? Placanje { get; set; }
    }

    public class RezervacijaCreateDto
    {
        public DateTime DatumVrijemeOd { get; set; }
        public DateTime DatumVrijemeDo { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public StatusRezervacije Status { get; set; }
        public int BrojSudionika { get; set; }

        [StringLength(500)]
        public string Napomena { get; set; } = string.Empty;

        public int KorisnikId { get; set; }
        public int ProstorId { get; set; }
    }

    public class RezervacijaUpdateDto : RezervacijaCreateDto
    {
    }

    public class ProstorSummaryDto
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = string.Empty;
        public decimal CijenaPoSatu { get; set; }
    }
}
