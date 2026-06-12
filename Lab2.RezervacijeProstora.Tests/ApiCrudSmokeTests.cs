using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Lab2.RezervacijeProstora.Dtos;
using Lab2.RezervacijeProstora.Models;

namespace Lab2.RezervacijeProstora.Tests
{
    public class ApiCrudSmokeTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public ApiCrudSmokeTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task KorisniciApi_ShouldSupportCrud()
        {
            var allResponse = await _client.GetAsync("/api/korisnici");
            allResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var queryResponse = await _client.GetAsync("/api/korisnici?query=Ivan");
            queryResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var getResponse = await _client.GetAsync("/api/korisnici/1");
            getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var missingResponse = await _client.GetAsync("/api/korisnici/999999");
            missingResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var createDto = new KorisnikCreateDto
            {
                KorisnickoIme = "api_korisnik",
                ImePrezime = "API Korisnik",
                Email = "api.korisnik@example.com",
                BrojTelefona = "+38591111111",
                DatumRegistracije = DateTime.UtcNow,
                TipKorisnika = TipKorisnika.Glazbenik
            };

            var created = await PostAndReadAsync<KorisnikCreateDto, KorisnikReadDto>("/api/korisnici", createDto);
            created.ImePrezime.Should().Be(createDto.ImePrezime);

            var invalidResponse = await _client.PostAsJsonAsync("/api/korisnici", new KorisnikCreateDto());
            invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateDto = new KorisnikUpdateDto
            {
                KorisnickoIme = "api_korisnik_edit",
                ImePrezime = "API Korisnik Edit",
                Email = "api.korisnik.edit@example.com",
                BrojTelefona = "+38592222222",
                DatumRegistracije = createDto.DatumRegistracije,
                TipKorisnika = TipKorisnika.Producent
            };

            var updateResponse = await _client.PutAsJsonAsync($"/api/korisnici/{created.Id}", updateDto);
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);

            var missingPutResponse = await _client.PutAsJsonAsync("/api/korisnici/999999", updateDto);
            missingPutResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var deleteResponse = await _client.DeleteAsync($"/api/korisnici/{created.Id}");
            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var missingDeleteResponse = await _client.DeleteAsync("/api/korisnici/999999");
            missingDeleteResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task VlasniciApi_ShouldSupportCrud()
        {
            (await _client.GetAsync("/api/vlasnici")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/vlasnici?query=Marko")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/vlasnici/1")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/vlasnici/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);

            var createDto = new VlasnikCreateDto
            {
                ImePrezime = "API Vlasnik",
                Email = "api.vlasnik@example.com",
                BrojTelefona = "+38593333333",
                DatumRegistracije = DateTime.UtcNow,
                Oib = "12312312312"
            };

            var created = await PostAndReadAsync<VlasnikCreateDto, VlasnikReadDto>("/api/vlasnici", createDto);
            created.ImePrezime.Should().Be(createDto.ImePrezime);

            var invalidResponse = await _client.PostAsJsonAsync("/api/vlasnici", new VlasnikCreateDto());
            invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateDto = new VlasnikUpdateDto
            {
                ImePrezime = "API Vlasnik Edit",
                Email = "api.vlasnik.edit@example.com",
                BrojTelefona = "+38594444444",
                DatumRegistracije = createDto.DatumRegistracije,
                Oib = "32132132132"
            };

            (await _client.PutAsJsonAsync($"/api/vlasnici/{created.Id}", updateDto)).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.PutAsJsonAsync("/api/vlasnici/999999", updateDto)).StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await _client.DeleteAsync($"/api/vlasnici/{created.Id}")).StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await _client.DeleteAsync("/api/vlasnici/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task OpremaApi_ShouldSupportCrud()
        {
            (await _client.GetAsync("/api/oprema")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/oprema?query=Shure")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/oprema/1")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/oprema/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);

            var createDto = new OpremaCreateDto
            {
                Naziv = "API Oprema",
                Proizvodac = "API Proizvodac",
                Ispravna = true,
                Vrijednost = 100
            };

            var created = await PostAndReadAsync<OpremaCreateDto, OpremaReadDto>("/api/oprema", createDto);
            created.Naziv.Should().Be(createDto.Naziv);

            var invalidResponse = await _client.PostAsJsonAsync("/api/oprema", new OpremaCreateDto());
            invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateDto = new OpremaUpdateDto
            {
                Naziv = "API Oprema Edit",
                Proizvodac = "API Proizvodac Edit",
                Ispravna = false,
                Vrijednost = 200
            };

            (await _client.PutAsJsonAsync($"/api/oprema/{created.Id}", updateDto)).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.PutAsJsonAsync("/api/oprema/999999", updateDto)).StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await _client.DeleteAsync($"/api/oprema/{created.Id}")).StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await _client.DeleteAsync("/api/oprema/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task ProstoriApi_ShouldSupportCrud()
        {
            (await _client.GetAsync("/api/prostori")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/prostori?query=Studio")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/prostori/1")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/prostori/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);

            var createDto = new ProstorZaProbuCreateDto
            {
                Naziv = "API Prostor",
                KapacitetOsoba = 5,
                CijenaPoSatu = 25,
                ImaParking = true,
                ImaKlimu = true,
                Aktivan = true,
                DatumDodavanja = DateTime.UtcNow,
                LokacijaId = 1,
                VlasnikId = 1,
                OpremaIds = new List<int> { 1 }
            };

            var created = await PostAndReadAsync<ProstorZaProbuCreateDto, ProstorZaProbuReadDto>("/api/prostori", createDto);
            created.Naziv.Should().Be(createDto.Naziv);

            var invalidResponse = await _client.PostAsJsonAsync("/api/prostori", new ProstorZaProbuCreateDto());
            invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateDto = new ProstorZaProbuUpdateDto
            {
                Naziv = "API Prostor Edit",
                KapacitetOsoba = 8,
                CijenaPoSatu = 35,
                ImaParking = false,
                ImaKlimu = true,
                Aktivan = true,
                DatumDodavanja = createDto.DatumDodavanja,
                LokacijaId = 1,
                VlasnikId = 1,
                OpremaIds = new List<int> { 1, 2 }
            };

            (await _client.PutAsJsonAsync($"/api/prostori/{created.Id}", updateDto)).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.PutAsJsonAsync("/api/prostori/999999", updateDto)).StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await _client.DeleteAsync($"/api/prostori/{created.Id}")).StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await _client.DeleteAsync("/api/prostori/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RezervacijeApi_ShouldSupportCrud()
        {
            (await _client.GetAsync("/api/rezervacije")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/rezervacije?query=Studio")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/rezervacije/1")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/rezervacije/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);

            var createDto = NewRezervacijaDto("API rezervacija");
            var created = await PostAndReadAsync<RezervacijaCreateDto, RezervacijaReadDto>("/api/rezervacije", createDto);
            created.Napomena.Should().Be(createDto.Napomena);

            var invalidDto = NewRezervacijaDto("Neispravna rezervacija");
            invalidDto.KorisnikId = 999999;
            var invalidResponse = await _client.PostAsJsonAsync("/api/rezervacije", invalidDto);
            invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateDto = NewRezervacijaDto("API rezervacija edit");
            (await _client.PutAsJsonAsync($"/api/rezervacije/{created.Id}", updateDto)).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.PutAsJsonAsync("/api/rezervacije/999999", updateDto)).StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await _client.DeleteAsync($"/api/rezervacije/{created.Id}")).StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await _client.DeleteAsync("/api/rezervacije/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task PlacanjaApi_ShouldSupportCrud()
        {
            (await _client.GetAsync("/api/placanja")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/placanja?query=TRX")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/placanja/1")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/placanja/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);

            var rezervacija = await CreateRezervacijaAsync("Rezervacija za placanje");
            var createDto = new PlacanjeCreateDto
            {
                Iznos = 150,
                DatumPlacanja = DateTime.UtcNow,
                Uspjesno = true,
                NacinPlacanja = NacinPlacanja.Kartica,
                BrojTransakcije = "API-TX-1",
                RezervacijaId = rezervacija.Id
            };

            var created = await PostAndReadAsync<PlacanjeCreateDto, PlacanjeReadDto>("/api/placanja", createDto);
            created.BrojTransakcije.Should().Be(createDto.BrojTransakcije);

            var invalidDto = new PlacanjeCreateDto { RezervacijaId = 999999 };
            var invalidResponse = await _client.PostAsJsonAsync("/api/placanja", invalidDto);
            invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateDto = new PlacanjeUpdateDto
            {
                Iznos = 175,
                DatumPlacanja = createDto.DatumPlacanja,
                Uspjesno = false,
                NacinPlacanja = NacinPlacanja.Transakcija,
                BrojTransakcije = "API-TX-2",
                RezervacijaId = rezervacija.Id
            };

            (await _client.PutAsJsonAsync($"/api/placanja/{created.Id}", updateDto)).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.PutAsJsonAsync("/api/placanja/999999", updateDto)).StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await _client.DeleteAsync($"/api/placanja/{created.Id}")).StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await _client.DeleteAsync("/api/placanja/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task RecenzijeApi_ShouldSupportCrud()
        {
            (await _client.GetAsync("/api/recenzije")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/recenzije?query=Odlican")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/recenzije/1")).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.GetAsync("/api/recenzije/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);

            var createDto = new RecenzijaCreateDto
            {
                Ocjena = 5,
                Komentar = "API recenzija",
                DatumRecenzije = DateTime.UtcNow,
                KorisnikId = 1,
                ProstorId = 1
            };

            var created = await PostAndReadAsync<RecenzijaCreateDto, RecenzijaReadDto>("/api/recenzije", createDto);
            created.Komentar.Should().Be(createDto.Komentar);

            var invalidResponse = await _client.PostAsJsonAsync("/api/recenzije", new RecenzijaCreateDto());
            invalidResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

            var updateDto = new RecenzijaUpdateDto
            {
                Ocjena = 4,
                Komentar = "API recenzija edit",
                DatumRecenzije = createDto.DatumRecenzije,
                KorisnikId = 1,
                ProstorId = 1
            };

            (await _client.PutAsJsonAsync($"/api/recenzije/{created.Id}", updateDto)).StatusCode.Should().Be(HttpStatusCode.OK);
            (await _client.PutAsJsonAsync("/api/recenzije/999999", updateDto)).StatusCode.Should().Be(HttpStatusCode.NotFound);
            (await _client.DeleteAsync($"/api/recenzije/{created.Id}")).StatusCode.Should().Be(HttpStatusCode.NoContent);
            (await _client.DeleteAsync("/api/recenzije/999999")).StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<TReadDto> PostAndReadAsync<TCreateDto, TReadDto>(string url, TCreateDto dto)
        {
            var response = await _client.PostAsJsonAsync(url, dto);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<TReadDto>();
            created.Should().NotBeNull();

            return created!;
        }

        private async Task<RezervacijaReadDto> CreateRezervacijaAsync(string napomena)
        {
            return await PostAndReadAsync<RezervacijaCreateDto, RezervacijaReadDto>(
                "/api/rezervacije",
                NewRezervacijaDto(napomena));
        }

        private static RezervacijaCreateDto NewRezervacijaDto(string napomena)
        {
            return new RezervacijaCreateDto
            {
                DatumVrijemeOd = DateTime.UtcNow.AddDays(10),
                DatumVrijemeDo = DateTime.UtcNow.AddDays(10).AddHours(2),
                DatumKreiranja = DateTime.UtcNow,
                Status = StatusRezervacije.Potvrdena,
                BrojSudionika = 3,
                Napomena = napomena,
                KorisnikId = 1,
                ProstorId = 1
            };
        }
    }
}
