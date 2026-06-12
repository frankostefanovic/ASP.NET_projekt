using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Dtos
{
    public static class DtoMappings
    {
        public static KorisnikReadDto ToReadDto(this Korisnik korisnik)
        {
            return new KorisnikReadDto
            {
                Id = korisnik.Id,
                KorisnickoIme = korisnik.KorisnickoIme,
                ImePrezime = korisnik.ImePrezime,
                Email = korisnik.Email,
                BrojTelefona = korisnik.BrojTelefona,
                DatumRegistracije = korisnik.DatumRegistracije,
                TipKorisnika = korisnik.TipKorisnika
            };
        }

        public static VlasnikReadDto ToReadDto(this Vlasnik vlasnik)
        {
            return new VlasnikReadDto
            {
                Id = vlasnik.Id,
                ImePrezime = vlasnik.ImePrezime,
                Email = vlasnik.Email,
                BrojTelefona = vlasnik.BrojTelefona,
                DatumRegistracije = vlasnik.DatumRegistracije,
                Oib = vlasnik.Oib
            };
        }

        public static LokacijaReadDto ToReadDto(this Lokacija lokacija)
        {
            return new LokacijaReadDto
            {
                Id = lokacija.Id,
                Grad = lokacija.Grad,
                Adresa = lokacija.Adresa,
                PostanskiBroj = lokacija.PostanskiBroj,
                Drzava = lokacija.Drzava
            };
        }

        public static OpremaReadDto ToReadDto(this Oprema oprema)
        {
            return new OpremaReadDto
            {
                Id = oprema.Id,
                Naziv = oprema.Naziv,
                Proizvodac = oprema.Proizvodac,
                Ispravna = oprema.Ispravna,
                Vrijednost = oprema.Vrijednost
            };
        }

        public static ProstorSummaryDto ToSummaryDto(this ProstorZaProbu prostor)
        {
            return new ProstorSummaryDto
            {
                Id = prostor.Id,
                Naziv = prostor.Naziv,
                CijenaPoSatu = prostor.CijenaPoSatu
            };
        }

        public static ProstorZaProbuReadDto ToReadDto(this ProstorZaProbu prostor)
        {
            return new ProstorZaProbuReadDto
            {
                Id = prostor.Id,
                Naziv = prostor.Naziv,
                KapacitetOsoba = prostor.KapacitetOsoba,
                CijenaPoSatu = prostor.CijenaPoSatu,
                ImaParking = prostor.ImaParking,
                ImaKlimu = prostor.ImaKlimu,
                Aktivan = prostor.Aktivan,
                DatumDodavanja = prostor.DatumDodavanja,
                Lokacija = prostor.Lokacija?.ToReadDto(),
                Vlasnik = prostor.Vlasnik?.ToReadDto(),
                Oprema = prostor.Oprema.Select(o => o.ToReadDto()).ToList(),
                Datoteke = prostor.Datoteke.Select(d => d.ToReadDto()).ToList()
            };
        }

        public static RezervacijaReadDto ToReadDto(this Rezervacija rezervacija)
        {
            return new RezervacijaReadDto
            {
                Id = rezervacija.Id,
                DatumVrijemeOd = rezervacija.DatumVrijemeOd,
                DatumVrijemeDo = rezervacija.DatumVrijemeDo,
                DatumKreiranja = rezervacija.DatumKreiranja,
                Status = rezervacija.Status,
                BrojSudionika = rezervacija.BrojSudionika,
                Napomena = rezervacija.Napomena,
                Korisnik = rezervacija.Korisnik?.ToReadDto(),
                Prostor = rezervacija.Prostor?.ToSummaryDto(),
                Placanje = rezervacija.Placanje?.ToReadDto()
            };
        }

        public static PlacanjeReadDto ToReadDto(this Placanje placanje)
        {
            return new PlacanjeReadDto
            {
                Id = placanje.Id,
                Iznos = placanje.Iznos,
                DatumPlacanja = placanje.DatumPlacanja,
                Uspjesno = placanje.Uspjesno,
                NacinPlacanja = placanje.NacinPlacanja,
                BrojTransakcije = placanje.BrojTransakcije,
                RezervacijaId = placanje.RezervacijaId
            };
        }

        public static RecenzijaReadDto ToReadDto(this Recenzija recenzija)
        {
            return new RecenzijaReadDto
            {
                Id = recenzija.Id,
                Ocjena = recenzija.Ocjena,
                Komentar = recenzija.Komentar,
                DatumRecenzije = recenzija.DatumRecenzije,
                Korisnik = recenzija.Korisnik?.ToReadDto(),
                Prostor = recenzija.Prostor?.ToSummaryDto()
            };
        }

        public static ProstorDatotekaReadDto ToReadDto(this ProstorDatoteka datoteka)
        {
            return new ProstorDatotekaReadDto
            {
                Id = datoteka.Id,
                ProstorZaProbuId = datoteka.ProstorZaProbuId,
                FileName = datoteka.FileName,
                FilePath = datoteka.FilePath,
                ContentType = datoteka.ContentType,
                FileSize = datoteka.FileSize,
                CreatedAt = datoteka.CreatedAt
            };
        }
    }
}
