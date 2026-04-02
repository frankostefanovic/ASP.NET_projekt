using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Lab1.RezervacijeProstora
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            DataLoader loader = new DataLoader();
            await loader.SimulirajUcitavanjePodatakaAsync();

            Vlasnik vlasnik1 = new Vlasnik(1, "Marko Horvat", "marko@gmail.com", "0911111111", new DateTime(2024, 1, 10), "12345678901");
            Vlasnik vlasnik2 = new Vlasnik(2, "Ana Kovač", "ana@gmail.com", "0922222222", new DateTime(2024, 2, 15), "23456789012");

            Lokacija lokacija1 = new Lokacija(1, "Zagreb", "Savska cesta 10", "10000", "Hrvatska");
            Lokacija lokacija2 = new Lokacija(2, "Zagreb", "Ilica 55", "10000", "Hrvatska");
            Lokacija lokacija3 = new Lokacija(3, "Velika Gorica", "Ulica kneza Branimira 8", "10410", "Hrvatska");

            Oprema oprema1 = new Oprema(1, "Bubnjevi Yamaha", "Yamaha", true, 1800);
            Oprema oprema2 = new Oprema(2, "Gitarsko pojačalo Marshall", "Marshall", true, 1200);
            Oprema oprema3 = new Oprema(3, "Mikseta Allen & Heath", "Allen & Heath", true, 2500);
            Oprema oprema4 = new Oprema(4, "Klavijature Roland", "Roland", false, 3000);
            Oprema oprema5 = new Oprema(5, "Mikrofoni Shure", "Shure", true, 900);

            ProstorZaProbu prostor1 = new ProstorZaProbu(
                1,
                "Studio Sonic",
                8,
                20,
                true,
                true,
                true,
                new DateTime(2025, 1, 5),
                lokacija1,
                vlasnik1
            );

            ProstorZaProbu prostor2 = new ProstorZaProbu(
                2,
                "Rehearsal Box",
                5,
                15,
                false,
                true,
                true,
                new DateTime(2025, 2, 12),
                lokacija2,
                vlasnik1
            );

            ProstorZaProbu prostor3 = new ProstorZaProbu(
                3,
                "Groove Room",
                10,
                25,
                true,
                false,
                true,
                new DateTime(2025, 3, 8),
                lokacija3,
                vlasnik2
            );

            vlasnik1.Prostori.Add(prostor1);
            vlasnik1.Prostori.Add(prostor2);
            vlasnik2.Prostori.Add(prostor3);

            prostor1.Oprema.Add(oprema1);
            prostor1.Oprema.Add(oprema2);
            prostor1.Oprema.Add(oprema5);

            prostor2.Oprema.Add(oprema2);
            prostor2.Oprema.Add(oprema3);

            prostor3.Oprema.Add(oprema1);
            prostor3.Oprema.Add(oprema3);
            prostor3.Oprema.Add(oprema4);
            prostor3.Oprema.Add(oprema5);

            oprema1.Prostori.Add(prostor1);
            oprema1.Prostori.Add(prostor3);

            oprema2.Prostori.Add(prostor1);
            oprema2.Prostori.Add(prostor2);

            oprema3.Prostori.Add(prostor2);
            oprema3.Prostori.Add(prostor3);

            oprema4.Prostori.Add(prostor3);

            oprema5.Prostori.Add(prostor1);
            oprema5.Prostori.Add(prostor3);

            Korisnik korisnik1 = new Korisnik(1, "rocker91", "Ivan Perić", "ivan@gmail.com", "0951111111", new DateTime(2025, 1, 20), TipKorisnika.Glazbenik);
            Korisnik korisnik2 = new Korisnik(2, "bend_x", "Bend X", "bendx@gmail.com", "0952222222", new DateTime(2025, 2, 10), TipKorisnika.Bend);
            Korisnik korisnik3 = new Korisnik(3, "producer_zg", "Petra Novak", "petra@gmail.com", "0953333333", new DateTime(2025, 2, 25), TipKorisnika.Producent);

            Rezervacija rezervacija1 = new Rezervacija(
                1,
                new DateTime(2026, 4, 5, 18, 0, 0),
                new DateTime(2026, 4, 5, 20, 0, 0),
                new DateTime(2026, 4, 1, 10, 15, 0),
                StatusRezervacije.Potvrdena,
                4,
                "Treba bubanj i 2 mikrofona.",
                korisnik1,
                prostor1
            );

            Rezervacija rezervacija2 = new Rezervacija(
                2,
                new DateTime(2026, 4, 6, 19, 0, 0),
                new DateTime(2026, 4, 6, 22, 0, 0),
                new DateTime(2026, 4, 1, 11, 30, 0),
                StatusRezervacije.NaCekanju,
                5,
                "Prva proba novog benda.",
                korisnik2,
                prostor2
            );

            Rezervacija rezervacija3 = new Rezervacija(
                3,
                new DateTime(2026, 4, 7, 17, 0, 0),
                new DateTime(2026, 4, 7, 21, 0, 0),
                new DateTime(2026, 4, 1, 12, 0, 0),
                StatusRezervacije.Potvrdena,
                3,
                "Produkcijska sesija.",
                korisnik3,
                prostor3
            );

            korisnik1.Rezervacije.Add(rezervacija1);
            korisnik2.Rezervacije.Add(rezervacija2);
            korisnik3.Rezervacije.Add(rezervacija3);

            prostor1.Rezervacije.Add(rezervacija1);
            prostor2.Rezervacije.Add(rezervacija2);
            prostor3.Rezervacije.Add(rezervacija3);

            Placanje placanje1 = new Placanje(1, rezervacija1.UkupnaCijena(), new DateTime(2026, 4, 1, 10, 20, 0), true, NacinPlacanja.Kartica, "TRX-1001");
            Placanje placanje2 = new Placanje(2, rezervacija2.UkupnaCijena(), new DateTime(2026, 4, 1, 11, 35, 0), false, NacinPlacanja.Transakcija, "TRX-1002");
            Placanje placanje3 = new Placanje(3, rezervacija3.UkupnaCijena(), new DateTime(2026, 4, 1, 12, 5, 0), true, NacinPlacanja.PayPal, "TRX-1003");

            rezervacija1.Placanje = placanje1;
            rezervacija2.Placanje = placanje2;
            rezervacija3.Placanje = placanje3;

            placanje1.Rezervacija = rezervacija1;
            placanje2.Rezervacija = rezervacija2;
            placanje3.Rezervacija = rezervacija3;

            Recenzija recenzija1 = new Recenzija(1, 5, "Odličan prostor i dobra oprema.", new DateTime(2026, 3, 20), korisnik1, prostor1);
            Recenzija recenzija2 = new Recenzija(2, 4, "Vrlo dobar prostor, ali parking je mali.", new DateTime(2026, 3, 22), korisnik2, prostor1);
            Recenzija recenzija3 = new Recenzija(3, 5, "Super akustika i odlična lokacija.", new DateTime(2026, 3, 25), korisnik3, prostor3);

            prostor1.Recenzije.Add(recenzija1);
            prostor1.Recenzije.Add(recenzija2);
            prostor3.Recenzije.Add(recenzija3);

            List<ProstorZaProbu> prostori = new List<ProstorZaProbu> { prostor1, prostor2, prostor3 };
            List<Korisnik> korisnici = new List<Korisnik> { korisnik1, korisnik2, korisnik3 };
            List<Rezervacija> rezervacije = new List<Rezervacija> { rezervacija1, rezervacija2, rezervacija3 };
            List<Oprema> svaOprema = new List<Oprema> { oprema1, oprema2, oprema3, oprema4, oprema5 };
            List<Vlasnik> vlasnici = new List<Vlasnik> { vlasnik1, vlasnik2 };

            Console.WriteLine("---- LINQ UPITI ----");

            var aktivniProstori = prostori
                .Where(p => p.Aktivan)
                .ToList();

            Console.WriteLine("\n1. Aktivni prostori:");
            foreach (var p in aktivniProstori)
            {
                Console.WriteLine($"{p.Naziv} - {p.Lokacija.Grad}");
            }

            var skupiProstori = prostori
                .Where(p => p.CijenaPoSatu > 18)
                .OrderByDescending(p => p.CijenaPoSatu)
                .ToList();

            Console.WriteLine("\n2. Prostori skuplji od 18 EUR po satu:");
            foreach (var p in skupiProstori)
            {
                Console.WriteLine($"{p.Naziv} - {p.CijenaPoSatu} EUR");
            }

            var potvrdeneRezervacije = rezervacije
                .Where(r => r.Status == StatusRezervacije.Potvrdena)
                .ToList();

            Console.WriteLine("\n3. Potvrđene rezervacije:");
            foreach (var r in potvrdeneRezervacije)
            {
                Console.WriteLine($"{r.Korisnik.ImePrezime} -> {r.Prostor.Naziv}");
            }

            var prostoriSaShureMikrofonima = prostori
                .Where(p => p.Oprema.Any(o => o.Naziv.Contains("Shure")))
                .ToList();

            Console.WriteLine("\n4. Prostori koji imaju Shure mikrofone:");
            foreach (var p in prostoriSaShureMikrofonima)
            {
                Console.WriteLine(p.Naziv);
            }

            var najboljiProstor = prostori
                .Where(p => p.Recenzije.Count > 0)
                .OrderByDescending(p => p.ProsjecnaOcjena())
                .FirstOrDefault();

            Console.WriteLine("\n5. Najbolje ocijenjeni prostor:");
            if (najboljiProstor != null)
            {
                Console.WriteLine($"{najboljiProstor.Naziv} - prosjek: {najboljiProstor.ProsjecnaOcjena()}");
            }

            var brojRezervacijaPoKorisniku = korisnici
                .Select(k => new
                {
                    Korisnik = k.ImePrezime,
                    BrojRezervacija = k.Rezervacije.Count
                })
                .OrderByDescending(x => x.BrojRezervacija)
                .ToList();

            Console.WriteLine("\n6. Broj rezervacija po korisniku:");
            foreach (var x in brojRezervacijaPoKorisniku)
            {
                Console.WriteLine($"{x.Korisnik} - {x.BrojRezervacija}");
            }

            var prostoriSNeispravnomOpremom = prostori
                .Where(p => p.Oprema.Any(o => o.Ispravna == false))
                .ToList();

            Console.WriteLine("\n7. Prostori s neispravnom opremom:");
            foreach (var p in prostoriSNeispravnomOpremom)
            {
                Console.WriteLine(p.Naziv);
            }

            var ukupnaZaradaPotvrdenihRezervacija = rezervacije
                .Where(r => r.Status == StatusRezervacije.Potvrdena)
                .Sum(r => r.UkupnaCijena());

            Console.WriteLine($"\n8. Ukupna zarada od potvrđenih rezervacija: {ukupnaZaradaPotvrdenihRezervacija} EUR");

            var prviProstorUZagrebu = prostori
                .FirstOrDefault(p => p.Lokacija.Grad == "Zagreb");

            Console.WriteLine("\n9. Prvi prostor u Zagrebu:");
            if (prviProstorUZagrebu != null)
            {
                Console.WriteLine(prviProstorUZagrebu.Naziv);
            }

            var vlasniciSViseProstora = vlasnici
                .Where(v => v.Prostori.Count > 1)
                .ToList();

            Console.WriteLine("\n10. Vlasnici s više od jednog prostora:");
            foreach (var v in vlasniciSViseProstora)
            {
                Console.WriteLine(v.ImePrezime);
            }
        }
    }
}