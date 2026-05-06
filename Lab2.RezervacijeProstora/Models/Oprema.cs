using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.RezervacijeProstora.Models
{
    public class Oprema
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        [Required]
        [StringLength(80)]
        public string Proizvodac { get; set; } = string.Empty;

        public bool Ispravna { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Vrijednost { get; set; }

        public virtual ICollection<ProstorZaProbu> Prostori { get; set; }

        public Oprema()
        {
            Prostori = new HashSet<ProstorZaProbu>();
        }

        public Oprema(int id, string naziv, string proizvodac, bool ispravna, decimal vrijednost)
        {
            Id = id;
            Naziv = naziv;
            Proizvodac = proizvodac;
            Ispravna = ispravna;
            Vrijednost = vrijednost;
            Prostori = new HashSet<ProstorZaProbu>();
        }
    }
}
