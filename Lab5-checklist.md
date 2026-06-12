# Lab 5 checklist

Predaja: 12.6.

## Bodovi i glavni zahtjevi

- [x] API podrska za sve entitete - 2 boda
- [x] Local authentication i authorization - 1 bod
- [x] Upload datoteka - 1 bod
- [x] 3rd party authentication - 1 bod
- [x] Integracijski testovi za API endpointe - 2 boda

## 1. API podrska za sve entitete

DTO sloj:

- [x] Napraviti Read DTO klase za sve glavne entitete
- [x] Napraviti Create DTO klase za sve glavne entitete
- [x] Napraviti Update DTO klase za sve glavne entitete
- [x] Napraviti mapper helper za pretvaranje EF entiteta u DTO
- [x] Za povezane podatke koristiti ugnijezdene DTO klase gdje ima smisla

Entiteti za koje treba napraviti API gdje CRUD ima smisla:

- [x] Korisnik
- [x] Vlasnik
- [x] Lokacija
- [x] ProstorZaProbu
- [x] Oprema
- [x] Rezervacija
- [x] Placanje
- [x] Recenzija

Za svaki API controller provjeriti:

- [x] Pilot `Lokacija` API koristi `[ApiController]`
- [x] Pilot `Lokacija` API koristi `[Route("api/...")]`
- [x] Pilot `Lokacija` API nasljeduje `ControllerBase`
- [x] Pilot `Lokacija` API ima `GET /api/lokacije`
- [x] Pilot `Lokacija` API ima `GET /api/lokacije?query=...`
- [x] Pilot `Lokacija` API ima `GET /api/lokacije/{id}`
- [x] Pilot `Lokacija` API ima `POST /api/lokacije`
- [x] Pilot `Lokacija` API ima `PUT /api/lokacije/{id}`
- [x] Pilot `Lokacija` API ima `DELETE /api/lokacije/{id}`
- [x] Pilot `Lokacija` API ne vraca EF entitet direktno
- [x] Pilot `Lokacija` API koristi DTO klase za ulaz i izlaz
- [x] Pilot `Lokacija` API vraca pravilne statuse: `200`, `201`, `204`, `400`, `404`
- [x] Isti API obrazac prosiriti na sve ostale entitete
- [x] Svi API controlleri koriste `[ApiController]`
- [x] Svi API controlleri koriste `[Route("api/...")]`
- [x] Svi API controlleri nasljeduju `ControllerBase`
- [x] Svi API controlleri imaju `GET /api/entitet`
- [x] Svi API controlleri imaju `GET /api/entitet?query=...`
- [x] Svi API controlleri imaju `GET /api/entitet/{id}`
- [x] Svi API controlleri imaju `POST /api/entitet`
- [x] Svi API controlleri imaju `PUT /api/entitet/{id}`
- [x] Svi API controlleri imaju `DELETE /api/entitet/{id}`
- [x] Svi API controlleri koriste DTO klase za ulaz i izlaz
- [x] Svi API controlleri ne vracaju EF entitete direktno
- [x] Povezani podaci se prikazuju kroz ugnijezdene DTO klase gdje ima smisla

## 2. Local authentication i authorization

- [x] Ukljuciti ASP.NET Core Identity
- [x] Napraviti `AppUser : IdentityUser`
- [x] Prosiriti `AppUser` poljima `OIB` i `JMBG`
- [x] Omoguciti registraciju
- [x] Omoguciti login
- [x] Omoguciti logout
- [x] Dodati role `Admin` i `Manager`
- [x] Seedati role u bazu
- [x] Omoguciti dodjelu role korisniku za testiranje
- [x] Dodati autorizacijska pravila na MVC akcije

Predlozena pravila:

- [x] `Index` i `Search` javno
- [x] `Details` samo prijavljeni korisnik
- [x] `Create` i `Edit` samo `Admin` ili `Manager`
- [x] `Delete` samo `Admin`

## 3. Upload datoteka

Upload vezati uz `ProstorZaProbu`.

Model:

- [x] Napraviti `ProstorDatoteka` ili `Attachment`
- [x] `Id`
- [x] `ProstorZaProbuId`
- [x] `FileName`
- [x] `FilePath`
- [x] `ContentType`
- [x] `FileSize`
- [x] `CreatedAt`

Funkcionalnosti:

- [x] Upload na Edit formi prostora, jer tada zapis ima ID
- [x] Upload raditi asinkrono preko Dropzone ili slicne odrzavane alternative
- [x] Datoteku spremiti na disk
- [x] U bazu spremiti metapodatke i putanju
- [x] Listu datoteka ucitati AJAX pozivom
- [x] Napraviti partial view za listu datoteka
- [x] Omoguciti brisanje postojece datoteke
- [x] Kod brisanja ukloniti zapis iz baze i datoteku s diska ako postoji

## 4. 3rd party authentication

Najprakticnije: Google login.

- [x] Provjeriti HTTPS profil u `launchSettings.json`
- [x] Dodati Google authentication paket
- [x] Konfigurirati `AddGoogle(...)`
- [x] ClientId spremiti u user secrets
- [x] ClientSecret spremiti u user secrets
- [x] Ne commitati tajne podatke
- [x] Google OAuth redirect vodi na Google login
- [x] Testirati vanjsku prijavu

## 5. Integracijski testovi za API

Napraviti testni projekt.

Paketi:

- [x] `xUnit`
- [x] `Microsoft.AspNetCore.Mvc.Testing`
- [x] `Microsoft.EntityFrameworkCore.InMemory`
- [x] `FluentAssertions`

Testna infrastruktura:

- [x] `WebApplicationFactory`
- [x] InMemory baza po testu ili po test klasi uz izolaciju
- [x] Seed minimalnih testnih podataka
- [x] Override konfiguracije za testove
- [x] Fake/mock samo za vanjske integracije gdje treba

Za svaki API controller minimalno testirati:

- [x] Pilot `Lokacija` API: `GET all` vraca `200`
- [x] Pilot `Lokacija` API: `GET by id` vraca `200` kada zapis postoji
- [x] Pilot `Lokacija` API: `GET by id` vraca `404` kada zapis ne postoji
- [x] Pilot `Lokacija` API: `POST` kreira zapis i vraca `201`
- [x] Pilot `Lokacija` API: `POST` vraca `400` za neispravan model
- [x] Pilot `Lokacija` API: `PUT` mijenja zapis
- [x] Pilot `Lokacija` API: `PUT` vraca `404` za nepostojeci zapis
- [x] Pilot `Lokacija` API: `DELETE` brise zapis
- [x] Pilot `Lokacija` API: `DELETE` vraca `404` za nepostojeci zapis
- [x] Prosiriti integracijske testove na sve ostale API controllere
- [x] Zasticeni endpointi vracaju odgovarajuci status bez autorizacije

## Moj redoslijed implementacije

1. Procitati PDF i napraviti ovu checklistu.
2. Dodati Identity modele, role i migraciju.
3. Dodati upload model i migraciju.
4. Napraviti DTO strukturu.
5. Napraviti pilot API za jednostavan entitet, npr. `Lokacija` ili `Oprema`.
6. Odmah napraviti testnu infrastrukturu i testove za pilot API.
7. Prosiriti API obrazac na sve ostale entitete.
8. Dodati autorizaciju po pravilima.
9. Dodati upload datoteka za `ProstorZaProbu`.
10. Prosiriti integracijske testove na sve API controllere.
11. Dodati Google login.
12. Rucno testirati sve zahtjeve prije commita.
