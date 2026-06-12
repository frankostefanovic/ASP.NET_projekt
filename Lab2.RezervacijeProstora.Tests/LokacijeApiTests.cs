using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Lab2.RezervacijeProstora.Dtos;

namespace Lab2.RezervacijeProstora.Tests
{
    public class LokacijeApiTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public LokacijeApiTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkAndLokacije()
        {
            var response = await _client.GetAsync("/api/lokacije");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var lokacije = await response.Content.ReadFromJsonAsync<List<LokacijaReadDto>>();
            lokacije.Should().NotBeNull();
            lokacije.Should().Contain(l => l.Grad == "Zagreb");
        }

        [Fact]
        public async Task GetAll_WithQuery_ShouldFilterLokacije()
        {
            var response = await _client.GetAsync("/api/lokacije?query=Zagreb");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var lokacije = await response.Content.ReadFromJsonAsync<List<LokacijaReadDto>>();
            lokacije.Should().NotBeNull();
            lokacije.Should().ContainSingle();
            lokacije![0].Grad.Should().Be("Zagreb");
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WhenLokacijaExists()
        {
            var response = await _client.GetAsync("/api/lokacije/1");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var lokacija = await response.Content.ReadFromJsonAsync<LokacijaReadDto>();
            lokacija.Should().NotBeNull();
            lokacija!.Id.Should().Be(1);
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenLokacijaDoesNotExist()
        {
            var response = await _client.GetAsync("/api/lokacije/999999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Post_ShouldCreateLokacijaAndReturnCreated()
        {
            var dto = new LokacijaCreateDto
            {
                Grad = "Test Grad",
                Adresa = "Test Adresa 1",
                PostanskiBroj = "99999",
                Drzava = "Hrvatska"
            };

            var response = await _client.PostAsJsonAsync("/api/lokacije", dto);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var created = await response.Content.ReadFromJsonAsync<LokacijaReadDto>();
            created.Should().NotBeNull();
            created!.Id.Should().BeGreaterThan(0);
            created.Grad.Should().Be(dto.Grad);
        }

        [Fact]
        public async Task Post_ShouldReturnBadRequest_WhenModelIsInvalid()
        {
            var dto = new LokacijaCreateDto
            {
                Grad = string.Empty,
                Adresa = string.Empty,
                PostanskiBroj = string.Empty,
                Drzava = string.Empty
            };

            var response = await _client.PostAsJsonAsync("/api/lokacije", dto);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_ShouldUpdateLokacija_WhenLokacijaExists()
        {
            var created = await CreateLokacijaAsync("Grad prije izmjene");
            var updateDto = new LokacijaUpdateDto
            {
                Grad = "Grad poslije izmjene",
                Adresa = "Nova adresa",
                PostanskiBroj = "11111",
                Drzava = "Hrvatska"
            };

            var response = await _client.PutAsJsonAsync($"/api/lokacije/{created.Id}", updateDto);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var updated = await response.Content.ReadFromJsonAsync<LokacijaReadDto>();
            updated.Should().NotBeNull();
            updated!.Grad.Should().Be(updateDto.Grad);
            updated.Adresa.Should().Be(updateDto.Adresa);
        }

        [Fact]
        public async Task Put_ShouldReturnNotFound_WhenLokacijaDoesNotExist()
        {
            var updateDto = new LokacijaUpdateDto
            {
                Grad = "Nepostojeci",
                Adresa = "Nema",
                PostanskiBroj = "00000",
                Drzava = "Hrvatska"
            };

            var response = await _client.PutAsJsonAsync("/api/lokacije/999999", updateDto);

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ShouldRemoveLokacija_WhenLokacijaExists()
        {
            var created = await CreateLokacijaAsync("Grad za brisanje");

            var deleteResponse = await _client.DeleteAsync($"/api/lokacije/{created.Id}");

            deleteResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);

            var getResponse = await _client.GetAsync($"/api/lokacije/{created.Id}");
            getResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Delete_ShouldReturnNotFound_WhenLokacijaDoesNotExist()
        {
            var response = await _client.DeleteAsync("/api/lokacije/999999");

            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        private async Task<LokacijaReadDto> CreateLokacijaAsync(string grad)
        {
            var dto = new LokacijaCreateDto
            {
                Grad = grad,
                Adresa = "Pomocna adresa",
                PostanskiBroj = "12345",
                Drzava = "Hrvatska"
            };

            var response = await _client.PostAsJsonAsync("/api/lokacije", dto);
            response.EnsureSuccessStatusCode();

            var created = await response.Content.ReadFromJsonAsync<LokacijaReadDto>();
            return created!;
        }
    }
}
