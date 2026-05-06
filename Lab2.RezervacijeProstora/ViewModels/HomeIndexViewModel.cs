using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.ViewModels
{
    public class HomeIndexViewModel
    {
        public int BrojKorisnika { get; set; }
        public int BrojProstora { get; set; }
        public int BrojRezervacija { get; set; }
        public int BrojOpreme { get; set; }

        public List<ProstorZaProbu> IstaknutiProstori { get; set; } = new List<ProstorZaProbu>();
        public List<Rezervacija> ZadnjeRezervacije { get; set; } = new List<Rezervacija>();
    }
}