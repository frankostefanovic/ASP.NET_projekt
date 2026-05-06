# Entity Framework Skill

Koristi ovaj skill kada treba mijenjati EF modele, DbContext, migracije ili LINQ upite u projektu `Lab2.RezervacijeProstora`.

## Pravila za modele

- Svaki model mora imati primarni ključ `Id`.
- Obavezna tekstualna polja označi s `[Required]`.
- Tekstualna polja ograniči s `[StringLength]`.
- Navigacijska svojstva postavi kao `virtual`.
- Kolekcijska navigacijska svojstva koristi kao `ICollection<T>`.
- Za veze dodaj eksplicitne strane ključeve, npr. `KorisnikId`, `ProstorId`, `VlasnikId`, `LokacijaId`.
- Zadrži defaultni konstruktor jer ga EF koristi za materijalizaciju objekata.
- Ne uklanjaj postojeće domenske metode kao `UkupnaCijena()`, `TrajanjeUSatima()` i `ProsjecnaOcjena()`.

## Pravila za DbContext

- Sve glavne tablice moraju imati `DbSet<T>`.
- Relacije konfiguriraj u `OnModelCreating` kada anotacije nisu dovoljne.
- Za `Rezervacija` i `Placanje` koristi one-to-one relaciju.
- Za `ProstorZaProbu` i `Oprema` koristi many-to-many relaciju.
- Kod višestrukih relacija prema istim tablicama pazi na `DeleteBehavior.Restrict` kako ne bi nastali cascade delete konflikti.

## Pravila za controllere

- Controlleri trebaju koristiti `ApplicationDbContext`, a ne mock repositoryje.
- Akcije koje čitaju podatke neka budu async.
- Koristi `ToListAsync`, `FirstOrDefaultAsync` i `CountAsync`.
- Uključi navigacijska svojstva s `.Include(...)` i `.ThenInclude(...)` kada ih view prikazuje.

## Pravila za migracije

- Nakon promjene modela pokreni:

```powershell
dotnet ef migrations add NazivMigracije
dotnet ef database update
```

- Nakon migracije pokreni:

```powershell
dotnet build
```

