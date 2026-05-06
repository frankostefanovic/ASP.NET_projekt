using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Models
{
    public class Vlasnik
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string Oib { get; set; } = string.Empty;

        public virtual ICollection<ProstorZaProbu> Prostori { get; set; }

        public Vlasnik()
        {
            Prostori = new HashSet<ProstorZaProbu>();
        }

        public Vlasnik(int id, string imePrezime, string email, string brojTelefona, DateTime datumRegistracije, string oib)
        {
            Id = id;
            ImePrezime = imePrezime;
            Email = email;
            BrojTelefona = brojTelefona;
            DatumRegistracije = datumRegistracije;
            Oib = oib;
            Prostori = new HashSet<ProstorZaProbu>();
        }
    }
}
