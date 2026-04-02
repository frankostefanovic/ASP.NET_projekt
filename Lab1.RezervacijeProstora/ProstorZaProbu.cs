using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Lab1.RezervacijeProstora
{
    public class ProstorZaProbu
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int KapacitetOsoba { get; set; }
        public decimal CijenaPoSatu { get; set; }
        public bool ImaParking { get; set; }
        public bool ImaKlimu { get; set; }
        public bool Aktivan { get; set; }
        public DateTime DatumDodavanja { get; set; }
        public Lokacija Lokacija { get; set; }
        public Vlasnik Vlasnik { get; set; }
        public List<Oprema> Oprema { get; set; }
        public List<Rezervacija> Rezervacije { get; set; }
        public List<Recenzija> Recenzije { get; set; }

        public ProstorZaProbu()
        {
            Oprema = new List<Oprema>();
            Rezervacije = new List<Rezervacija>();
            Recenzije = new List<Recenzija>();
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
            Oprema = new List<Oprema>();
            Rezervacije = new List<Rezervacija>();
            Recenzije = new List<Recenzija>();
        }

        public decimal ProsjecnaOcjena()
        {
            if (Recenzije.Count == 0)
                return 0;

            return (decimal)Recenzije.Average(r => r.Ocjena);
        }
    }
}
