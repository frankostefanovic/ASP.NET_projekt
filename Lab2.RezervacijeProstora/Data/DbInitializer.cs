using Lab2.RezervacijeProstora.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab2.RezervacijeProstora.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(ApplicationDbContext context)
        {
            await context.Database.MigrateAsync();

            if (await context.Prostori.AnyAsync())
                return;

            var lok1 = new Lokacija(1, "Zagreb", "Ilica 10", "10000", "Hrvatska");
            var lok2 = new Lokacija(2, "Split", "Riva 5", "21000", "Hrvatska");
            var lok3 = new Lokacija(3, "Rijeka", "Korzo 1", "51000", "Hrvatska");

            var vlas1 = new Vlasnik(1, "Marko Horvat", "marko@example.com", "+38591123456", DateTime.Now.AddYears(-2), "12345678901");
            var vlas2 = new Vlasnik(2, "Ana Kovac", "ana@example.com", "+38591567890", DateTime.Now.AddYears(-1), "10987654321");
            var vlas3 = new Vlasnik(3, "Studio X", "studiox@example.com", "+38591200000", DateTime.Now.AddMonths(-6), "55566677788");

            var op1 = new Oprema(1, "Mikrofon Shure SM58", "Shure", true, 1200m);
            var op2 = new Oprema(2, "Pojacalo Fender", "Fender", true, 4500m);
            var op3 = new Oprema(3, "Mikseta Behringer", "Behringer", true, 2500m);
            var op4 = new Oprema(4, "Bas gitara", "Yamaha", false, 2000m);

            var p1 = new ProstorZaProbu(1, "Studio A", 6, 50m, true, true, true, DateTime.Now.AddMonths(-8), lok1, vlas1);
            var p2 = new ProstorZaProbu(2, "Studio B", 10, 75m, true, false, true, DateTime.Now.AddMonths(-5), lok1, vlas2);
            var p3 = new ProstorZaProbu(3, "Rehearsal Room Split", 8, 60m, false, true, true, DateTime.Now.AddMonths(-12), lok2, vlas3);
            var p4 = new ProstorZaProbu(4, "Basement Studio", 4, 30m, false, false, false, DateTime.Now.AddMonths(-2), lok3, vlas1);

            p1.Oprema.Add(op1);
            p1.Oprema.Add(op3);
            p2.Oprema.Add(op2);
            p2.Oprema.Add(op3);
            p3.Oprema.Add(op1);
            p3.Oprema.Add(op4);

            var k1 = new Korisnik(1, "ivan123", "Ivan Ivic", "ivan@example.com", "+38591234567", DateTime.Now.AddYears(-1), TipKorisnika.Glazbenik);
            var k2 = new Korisnik(2, "grupa_rock", "Rock Band", "band@example.com", "+38591999999", DateTime.Now.AddMonths(-6), TipKorisnika.Bend);
            var k3 = new Korisnik(3, "producent1", "Petra Producent", "petra@example.com", "+38591888888", DateTime.Now.AddMonths(-3), TipKorisnika.Producent);

            var r1 = new Rezervacija(1, DateTime.Now.AddDays(3).Date.AddHours(18), DateTime.Now.AddDays(3).Date.AddHours(21), DateTime.Now, StatusRezervacije.Potvrdena, 4, "Probe za album", k1, p1);
            var r2 = new Rezervacija(2, DateTime.Now.AddDays(7).Date.AddHours(10), DateTime.Now.AddDays(7).Date.AddHours(13), DateTime.Now.AddDays(-1), StatusRezervacije.NaCekanju, 6, "Generalna proba", k2, p2);
            var r3 = new Rezervacija(3, DateTime.Now.AddDays(-10).Date.AddHours(14), DateTime.Now.AddDays(-10).Date.AddHours(17), DateTime.Now.AddDays(-20), StatusRezervacije.Zavrsena, 3, "Snimanje", k3, p3);
            var r4 = new Rezervacija(4, DateTime.Now.AddDays(1).Date.AddHours(9), DateTime.Now.AddDays(1).Date.AddHours(11), DateTime.Now, StatusRezervacije.Potvrdena, 2, "Pokusaj opreme", k1, p3);

            var pay1 = new Placanje(1, r1.UkupnaCijena(), DateTime.Now.AddDays(-1), true, NacinPlacanja.Transakcija, "TRX12345")
            {
                Rezervacija = r1,
                RezervacijaId = r1.Id
            };
            var pay2 = new Placanje(2, r3.UkupnaCijena(), DateTime.Now.AddDays(-9), true, NacinPlacanja.Kartica, "CARD9876")
            {
                Rezervacija = r3,
                RezervacijaId = r3.Id
            };
            var pay3 = new Placanje(3, r4.UkupnaCijena(), DateTime.Now, false, NacinPlacanja.Gotovina, string.Empty)
            {
                Rezervacija = r4,
                RezervacijaId = r4.Id
            };

            r1.Placanje = pay1;
            r3.Placanje = pay2;
            r4.Placanje = pay3;

            var rec1 = new Recenzija(1, 5, "Odlican prostor, dobra oprema.", DateTime.Now.AddDays(-5), k3, p1);
            var rec2 = new Recenzija(2, 4, "Fino mjesto, ali malo skuplje.", DateTime.Now.AddDays(-2), k1, p2);
            var rec3 = new Recenzija(3, 3, "Osnovno, nema parkiranja.", DateTime.Now.AddDays(-1), k2, p4);

            context.AddRange(lok1, lok2, lok3);
            context.AddRange(vlas1, vlas2, vlas3);
            context.AddRange(op1, op2, op3, op4);
            context.AddRange(p1, p2, p3, p4);
            context.AddRange(k1, k2, k3);
            context.AddRange(r1, r2, r3, r4);
            context.AddRange(pay1, pay2, pay3);
            context.AddRange(rec1, rec2, rec3);

            await context.SaveChangesAsync();
        }
    }
}
