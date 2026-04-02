using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    public class Placanje
    {
        public int Id { get; set; }
        public decimal Iznos { get; set; }
        public DateTime DatumPlacanja { get; set; }
        public bool Uspjesno { get; set; }
        public NacinPlacanja NacinPlacanja { get; set; }
        public string BrojTransakcije { get; set; }
        public Rezervacija Rezervacija { get; set; }

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