using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Models
{
    public class Korisnik
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string KorisnickoIme { get; set; } = string.Empty;

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
        public TipKorisnika TipKorisnika { get; set; }

        public virtual ICollection<Rezervacija> Rezervacije { get; set; }
        public virtual ICollection<Recenzija> Recenzije { get; set; }

        public Korisnik()
        {
            Rezervacije = new HashSet<Rezervacija>();
            Recenzije = new HashSet<Recenzija>();
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
            Rezervacije = new HashSet<Rezervacija>();
            Recenzije = new HashSet<Recenzija>();
        }
    }
}
