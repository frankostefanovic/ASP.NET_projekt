using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    public class Recenzija
    {
        public int Id { get; set; }
        public int Ocjena { get; set; }
        public string Komentar { get; set; }
        public DateTime DatumRecenzije { get; set; }
        public Korisnik Korisnik { get; set; }
        public ProstorZaProbu Prostor { get; set; }

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
        }
    }
}