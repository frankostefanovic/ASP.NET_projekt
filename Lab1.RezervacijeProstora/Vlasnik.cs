using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    public class Vlasnik
    {
        public int Id { get; set; }
        public string ImePrezime { get; set; }
        public string Email { get; set; }
        public string BrojTelefona { get; set; }
        public DateTime DatumRegistracije { get; set; }
        public string Oib { get; set; }
        public List<ProstorZaProbu> Prostori { get; set; }

        public Vlasnik()
        {
            Prostori = new List<ProstorZaProbu>();
        }

        public Vlasnik(int id, string imePrezime, string email, string brojTelefona, DateTime datumRegistracije, string oib)
        {
            Id = id;
            ImePrezime = imePrezime;
            Email = email;
            BrojTelefona = brojTelefona;
            DatumRegistracije = datumRegistracije;
            Oib = oib;
            Prostori = new List<ProstorZaProbu>();
        }
    }
}
