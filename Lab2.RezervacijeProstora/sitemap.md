# Sitemap - Routing Model

Ovaj dokument opisuje dostupne URL-ove, MVC controllere, akcije i viewove u aplikaciji.

## Custom rute

| URL | Controller | Akcija | View |
| --- | --- | --- | --- |
| `/prostori` | `ProstorZaProbuController` | `Index` | `Views/ProstorZaProbu/Index.cshtml` |
| `/prostori/detalji/{id}` | `ProstorZaProbuController` | `Details(int id)` | `Views/ProstorZaProbu/Details.cshtml` |
| `/rezervacije` | `RezervacijaController` | `Index` | `Views/Rezervacija/Index.cshtml` |
| `/korisnici/detalji/{id}` | `KorisnikController` | `Details(int id)` | `Views/Korisnik/Details.cshtml` |

## Default MVC ruta

Default ruta je definirana kao:

```csharp
{controller=Home}/{action=Index}/{id?}
```

Ako URL ne odgovara custom rutama, aplikacija koristi standardni MVC obrazac `/Controller/Action/{id?}`.

## Standardne stranice

| URL | Controller | Akcija | View |
| --- | --- | --- | --- |
| `/` | `HomeController` | `Index` | `Views/Home/Index.cshtml` |
| `/Home/Index` | `HomeController` | `Index` | `Views/Home/Index.cshtml` |
| `/Home/Privacy` | `HomeController` | `Privacy` | `Views/Home/Privacy.cshtml` |
| `/Korisnik` | `KorisnikController` | `Index` | `Views/Korisnik/Index.cshtml` |
| `/Korisnik/Details/{id}` | `KorisnikController` | `Details(int id)` | `Views/Korisnik/Details.cshtml` |
| `/ProstorZaProbu` | `ProstorZaProbuController` | `Index` | `Views/ProstorZaProbu/Index.cshtml` |
| `/ProstorZaProbu/Details/{id}` | `ProstorZaProbuController` | `Details(int id)` | `Views/ProstorZaProbu/Details.cshtml` |
| `/Rezervacija` | `RezervacijaController` | `Index` | `Views/Rezervacija/Index.cshtml` |
| `/Rezervacija/Details/{id}` | `RezervacijaController` | `Details(int id)` | `Views/Rezervacija/Details.cshtml` |
| `/Oprema` | `OpremaController` | `Index` | `Views/Oprema/Index.cshtml` |
| `/Oprema/Details/{id}` | `OpremaController` | `Details(int id)` | `Views/Oprema/Details.cshtml` |
| `/Placanje` | `PlacanjeController` | `Index` | `Views/Placanje/Index.cshtml` |
| `/Placanje/Details/{id}` | `PlacanjeController` | `Details(int id)` | `Views/Placanje/Details.cshtml` |
| `/Recenzija` | `RecenzijaController` | `Index` | `Views/Recenzija/Index.cshtml` |
| `/Recenzija/Details/{id}` | `RecenzijaController` | `Details(int id)` | `Views/Recenzija/Details.cshtml` |
| `/Vlasnik` | `VlasnikController` | `Index` | `Views/Vlasnik/Index.cshtml` |
| `/Vlasnik/Details/{id}` | `VlasnikController` | `Details(int id)` | `Views/Vlasnik/Details.cshtml` |
| `/Lokacija` | `LokacijaController` | `Index` | `Views/Lokacija/Index.cshtml` |
| `/Lokacija/Details/{id}` | `LokacijaController` | `Details(int id)` | `Views/Lokacija/Details.cshtml` |

