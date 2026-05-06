using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Lab2.RezervacijeProstora.Models
{
    public class ProstorZaProbu
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Naziv { get; set; } = string.Empty;

        public int KapacitetOsoba { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CijenaPoSatu { get; set; }

        public bool ImaParking { get; set; }
        public bool ImaKlimu { get; set; }
        public bool Aktivan { get; set; }
        public DateTime DatumDodavanja { get; set; }

        public int LokacijaId { get; set; }
        public int VlasnikId { get; set; }

        [ForeignKey(nameof(LokacijaId))]
        public virtual Lokacija Lokacija { get; set; } = null!;

        [ForeignKey(nameof(VlasnikId))]
        public virtual Vlasnik Vlasnik { get; set; } = null!;

        public virtual ICollection<Oprema> Oprema { get; set; }
        public virtual ICollection<Rezervacija> Rezervacije { get; set; }
        public virtual ICollection<Recenzija> Recenzije { get; set; }

        public ProstorZaProbu()
        {
            Oprema = new HashSet<Oprema>();
            Rezervacije = new HashSet<Rezervacija>();
            Recenzije = new HashSet<Recenzija>();
        }

        public ProstorZaProbu(int id, string naziv, int kapacitetOsoba, decimal cijenaPoSatu, bool imaParking, bool imaKlimu, bool aktivan, DateTime datumDodavanja, Lokacija lokacija, Vlasnik vlasnik)
        {
            Id = id;
            Naziv = naziv;
            KapacitetOsoba = kapacitetOsoba;
            CijenaPoSatu = cijenaPoSatu;
            ImaParking = imaParking;
            ImaKlimu = imaKlimu;
            Aktivan = aktivan;
            DatumDodavanja = datumDodavanja;
            Lokacija = lokacija;
            Vlasnik = vlasnik;
            LokacijaId = lokacija.Id;
            VlasnikId = vlasnik.Id;
            Oprema = new HashSet<Oprema>();
            Rezervacije = new HashSet<Rezervacija>();
            Recenzije = new HashSet<Recenzija>();
        }

        public decimal ProsjecnaOcjena()
        {
            if (Recenzije.Count == 0)
                return 0;

            return (decimal)Recenzije.Average(r => r.Ocjena);
        }
    }
}
