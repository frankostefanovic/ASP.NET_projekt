using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab2.RezervacijeProstora.Models
{
    public class Placanje
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Iznos { get; set; }

        public DateTime DatumPlacanja { get; set; }
        public bool Uspjesno { get; set; }
        public NacinPlacanja NacinPlacanja { get; set; }

        [StringLength(80)]
        public string BrojTransakcije { get; set; } = string.Empty;

        public int RezervacijaId { get; set; }

        [ForeignKey(nameof(RezervacijaId))]
        public virtual Rezervacija Rezervacija { get; set; } = null!;

        public Placanje()
        {
        }

        public Placanje(int id, decimal iznos, DateTime datumPlacanja, bool uspjesno, NacinPlacanja nacinPlacanja, string brojTransakcije)
        {
            Id = id;
            Iznos = iznos;
            DatumPlacanja = datumPlacanja;
            Uspjesno = uspjesno;
            NacinPlacanja = nacinPlacanja;
            BrojTransakcije = brojTransakcije;
        }
    }
}
