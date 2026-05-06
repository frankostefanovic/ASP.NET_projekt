# Semantic Model - Rezervacije Prostora

Ovaj dokument opisuje glavne domenske klase/tablice, njihova svojstva i veze u aplikaciji za rezervacije prostora za probu.

## Tablice i modeli

### Korisnik

Predstavlja osobu, bend ili producenta koji koristi aplikaciju i radi rezervacije.

- `Id` - primarni ključ
- `KorisnickoIme` - korisničko ime
- `ImePrezime` - ime osobe ili naziv benda
- `Email` - kontakt email
- `BrojTelefona` - kontakt telefon
- `DatumRegistracije` - datum registracije korisnika
- `TipKorisnika` - enum vrijednost: glazbenik, bend ili producent

Veze:

- jedan korisnik ima više rezervacija
- jedan korisnik može napisati više recenzija

### ProstorZaProbu

Predstavlja prostor koji se može rezervirati.

- `Id` - primarni ključ
- `Naziv` - naziv prostora
- `KapacitetOsoba` - maksimalan broj osoba
- `CijenaPoSatu` - cijena najma po satu
- `ImaParking` - ima li prostor parking
- `ImaKlimu` - ima li prostor klimu
- `Aktivan` - je li prostor aktivan za rezerviranje
- `DatumDodavanja` - datum dodavanja prostora
- `LokacijaId` - strani ključ prema lokaciji
- `VlasnikId` - strani ključ prema vlasniku

Veze:

- jedan prostor pripada jednoj lokaciji
- jedan prostor pripada jednom vlasniku
- jedan prostor ima više rezervacija
- jedan prostor ima više recenzija
- jedan prostor može imati više komada opreme

### Rezervacija

Predstavlja termin rezervacije prostora.

- `Id` - primarni ključ
- `DatumVrijemeOd` - početak termina
- `DatumVrijemeDo` - kraj termina
- `DatumKreiranja` - vrijeme kreiranja rezervacije
- `Status` - enum status rezervacije
- `BrojSudionika` - broj sudionika probe
- `Napomena` - dodatna napomena
- `KorisnikId` - strani ključ prema korisniku
- `ProstorId` - strani ključ prema prostoru

Veze:

- jedna rezervacija pripada jednom korisniku
- jedna rezervacija pripada jednom prostoru
- jedna rezervacija može imati jedno plaćanje

### Placanje

Predstavlja plaćanje vezano uz rezervaciju.

- `Id` - primarni ključ
- `Iznos` - iznos plaćanja
- `DatumPlacanja` - datum plaćanja
- `Uspjesno` - je li plaćanje uspješno
- `NacinPlacanja` - enum način plaćanja
- `BrojTransakcije` - identifikator transakcije
- `RezervacijaId` - strani ključ prema rezervaciji

Veza:

- jedno plaćanje pripada jednoj rezervaciji

### Recenzija

Predstavlja ocjenu i komentar korisnika za prostor.

- `Id` - primarni ključ
- `Ocjena` - ocjena od 1 do 5
- `Komentar` - tekst recenzije
- `DatumRecenzije` - datum recenzije
- `KorisnikId` - strani ključ prema korisniku
- `ProstorId` - strani ključ prema prostoru

Veze:

- jedna recenzija pripada jednom korisniku
- jedna recenzija pripada jednom prostoru

### Oprema

Predstavlja opremu dostupnu u prostorima.

- `Id` - primarni ključ
- `Naziv` - naziv opreme
- `Proizvodac` - proizvođač
- `Ispravna` - je li oprema ispravna
- `Vrijednost` - procijenjena vrijednost opreme

Veza:

- oprema može biti dostupna u više prostora
- prostor može imati više komada opreme

### Vlasnik

Predstavlja vlasnika prostora.

- `Id` - primarni ključ
- `ImePrezime` - ime vlasnika ili naziv organizacije
- `Email` - kontakt email
- `BrojTelefona` - kontakt telefon
- `DatumRegistracije` - datum registracije vlasnika
- `Oib` - OIB vlasnika

Veza:

- jedan vlasnik može imati više prostora

### Lokacija

Predstavlja adresu prostora.

- `Id` - primarni ključ
- `Grad` - grad
- `Adresa` - adresa
- `PostanskiBroj` - poštanski broj
- `Drzava` - država

Veza:

- jedna lokacija može imati više prostora

## Glavne relacije

- `Korisnik 1:N Rezervacija`
- `Korisnik 1:N Recenzija`
- `Vlasnik 1:N ProstorZaProbu`
- `Lokacija 1:N ProstorZaProbu`
- `ProstorZaProbu 1:N Rezervacija`
- `ProstorZaProbu 1:N Recenzija`
- `Rezervacija 1:1 Placanje`
- `ProstorZaProbu N:N Oprema`

