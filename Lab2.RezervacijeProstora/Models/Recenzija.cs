using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.RezervacijeProstora.Models
{
    public class Recenzija
    {
        [Key]
        public int Id { get; set; }

        [Range(1, 5)]
        public int Ocjena { get; set; }

        [Required]
        [StringLength(500)]
        public string Komentar { get; set; } = string.Empty;

        public DateTime DatumRecenzije { get; set; }

        public int KorisnikId { get; set; }
        public int ProstorId { get; set; }

        [ForeignKey(nameof(KorisnikId))]
        public virtual Korisnik Korisnik { get; set; } = null!;

        [ForeignKey(nameof(ProstorId))]
        public virtual ProstorZaProbu Prostor { get; set; } = null!;

        public Recenzija()
        {
        }

        public Recenzija(int id, int ocjena, string komentar, DateTime datumRecenzije, Korisnik korisnik, ProstorZaProbu prostor)
        {
            Id = id;
            Ocjena = ocjena;
            Komentar = komentar;
            DatumRecenzije = datumRecenzije;
            Korisnik = korisnik;
            Prostor = prostor;
            KorisnikId = korisnik.Id;
            ProstorId = prostor.Id;
        }
    }
}
