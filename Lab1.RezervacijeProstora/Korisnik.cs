using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    public class Korisnik
    {
        public int Id { get; set; }
        public string KorisnickoIme { get; set; }
        public string ImePrezime { get; set; }
        public string Email { get; set; }
        public string BrojTelefona { get; set; }
        public DateTime DatumRegistracije { get; set; }
        public TipKorisnika TipKorisnika { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }

        public Korisnik()
        {
            Rezervacije = new List<Rezervacija>();
        }

        public Korisnik(int id, string korisnickoIme, string imePrezime, string email, string brojTelefona, DateTime datumRegistracije, TipKorisnika tipKorisnika)
        {
            Id = id;
            KorisnickoIme = korisnickoIme;
            ImePrezime = imePrezime;
            Email = email;
            BrojTelefona = brojTelefona;
            DatumRegistracije = datumRegistracije;
            TipKorisnika = tipKorisnika;
            Rezervacije = new List<Rezervacija>();
        }
    }
}