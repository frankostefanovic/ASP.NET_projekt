using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    public class Rezervacija
    {
        public int Id { get; set; }
        public DateTime DatumVrijemeOd { get; set; }
        public DateTime DatumVrijemeDo { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public StatusRezervacije Status { get; set; }
        public int BrojSudionika { get; set; }
        public string Napomena { get; set; }
        public Korisnik Korisnik { get; set; }
        public ProstorZaProbu Prostor { get; set; }
        public Placanje Placanje { get; set; }

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
        }

        public double TrajanjeUSatima()
        {
            return (DatumVrijemeDo - DatumVrijemeOd).TotalHours;
        }

        public decimal UkupnaCijena()
        {
            return (decimal)TrajanjeUSatima() * Prostor.CijenaPoSatu;
        }
    }
}