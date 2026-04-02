using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1.RezervacijeProstora
{
    public class Lokacija
    {
        public int Id { get; set; }
        public string Grad { get; set; }
        public string Adresa { get; set; }
        public string PostanskiBroj { get; set; }
        public string Drzava { get; set; }

        public Lokacija()
        {
        }

        public Lokacija(int id, string grad, string adresa, string postanskiBroj, string drzava)
        {
            Id = id;
            Grad = grad;
            Adresa = adresa;
            PostanskiBroj = postanskiBroj;
            Drzava = drzava;
        }

        public override string ToString()
        {
            return $"{Adresa}, {Grad}, {PostanskiBroj}, {Drzava}";
        }
    }
}
