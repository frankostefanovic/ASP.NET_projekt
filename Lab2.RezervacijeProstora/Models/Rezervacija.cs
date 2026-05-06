using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.RezervacijeProstora.Models
{
    public class Rezervacija
    {
        [Key]
        public int Id { get; set; }

        public DateTime DatumVrijemeOd { get; set; }
        public DateTime DatumVrijemeDo { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public StatusRezervacije Status { get; set; }
        public int BrojSudionika { get; set; }

        [StringLength(500)]
        public string Napomena { get; set; } = string.Empty;

        public int KorisnikId { get; set; }
        public int ProstorId { get; set; }

        [ForeignKey(nameof(KorisnikId))]
        public virtual Korisnik Korisnik { get; set; } = null!;

        [ForeignKey(nameof(ProstorId))]
        public virtual ProstorZaProbu Prostor { get; set; } = null!;

        public virtual Placanje? Placanje { get; set; }

        public Rezervacija()
        {
        }

        public Rezervacija(int id, DateTime datumVrijemeOd, DateTime datumVrijemeDo, DateTime datumKreiranja, StatusRezervacije status, int brojSudionika, string napomena, Korisnik korisnik, ProstorZaProbu prostor)
        {
            Id = id;
            DatumVrijemeOd = datumVrijemeOd;
            DatumVrijemeDo = datumVrijemeDo;
            DatumKreiranja = datumKreiranja;
            Status = status;
            BrojSudionika = brojSudionika;
            Napomena = napomena;
            Korisnik = korisnik;
            Prostor = prostor;
            KorisnikId = korisnik.Id;
            ProstorId = prostor.Id;
        }

        public double TrajanjeUSatima()
        {
            return (DatumVrijemeDo - DatumVrijemeOd).TotalHours;
        }

        public decimal UkupnaCijena()
        {
            if (Prostor == null)
                return 0;

            return (decimal)TrajanjeUSatima() * Prostor.CijenaPoSatu;
        }
    }
}
