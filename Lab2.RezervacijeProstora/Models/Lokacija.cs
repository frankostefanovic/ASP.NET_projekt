using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab2.RezervacijeProstora.Models
{
    public class Lokacija
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(80)]
        public string Grad { get; set; } = string.Empty;

        [Required]
        [StringLength(120)]
        public string Adresa { get; set; } = string.Empty;

        [Required]
        [StringLength(12)]
        public string PostanskiBroj { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Drzava { get; set; } = string.Empty;

        public virtual ICollection<ProstorZaProbu> Prostori { get; set; }

        public Lokacija()
        {
            Prostori = new HashSet<ProstorZaProbu>();
        }

        public Lokacija(int id, string grad, string adresa, string postanskiBroj, string drzava)
        {
            Id = id;
            Grad = grad;
            Adresa = adresa;
            PostanskiBroj = postanskiBroj;
            Drzava = drzava;
            Prostori = new HashSet<ProstorZaProbu>();
        }

        public override string ToString()
        {
            return $"{Adresa}, {Grad}, {PostanskiBroj}, {Drzava}";
        }
    }
}
