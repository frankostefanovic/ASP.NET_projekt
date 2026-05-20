# Lab 4 checklist

## Implementirano

- CRUD akcije i viewevi za sve glavne entitete:
  - Korisnik
  - Lokacija
  - Oprema
  - Vlasnik
  - ProstorZaProbu
  - Rezervacija
  - Recenzija
  - Placanje
- AJAX pretraga na svim Index listama bez reloadanja stranice.
- Partial viewevi za prikaz rezultata listi.
- Autocomplete dropdown koji dohvaća prijedloge sa servera AJAX-om.
- Custom date picker kao shared partial view, bez browser date/datetime-local kontrole.
- Client-side validacija na blur/focusout.
- Agent log zapisi u `.github/hooks/agent_log`.

## Kako testirati

1. Pokrenuti aplikaciju:

   ```powershell
   dotnet run --urls http://localhost:5097
   ```

2. Otvoriti `http://localhost:5097`.
3. Na svakoj listi upisati pojam u pretragu i provjeriti da se kartice mijenjaju bez reloadanja stranice.
4. U formama s povezanim entitetima upisati pojam u autocomplete i odabrati prijedlog.
5. U datumskim poljima kliknuti `Odaberi` i koristiti custom kalendar.
6. Ostaviti obavezno polje prazno i kliknuti izvan polja kako bi se validacija prikazala na blur.
7. Kliknuti `Spremi` i provjeriti da se zapis pojavi na listi.

## Napomena

Kod autocomplete polja potrebno je odabrati prijedlog iz liste ili imati jednoznačan prijedlog koji se automatski odabere na blur. Server sprema ID odabranog zapisa, a ne samo tekst koji je upisan u polje.
