using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    public class Oprema
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Proizvodac { get; set; }
        public bool Ispravna { get; set; }
        public decimal Vrijednost { get; set; }
        public List<ProstorZaProbu> Prostori { get; set; }

        public Oprema()
        {
            Prostori = new List<ProstorZaProbu>();
        }

        public Oprema(int id, string naziv, string proizvodac, bool ispravna, decimal vrijednost)
        {
            Id = id;
            Naziv = naziv;
            Proizvodac = proizvodac;
            Ispravna = ispravna;
            Vrijednost = vrijednost;
            Prostori = new List<ProstorZaProbu>();
        }
    }
}
