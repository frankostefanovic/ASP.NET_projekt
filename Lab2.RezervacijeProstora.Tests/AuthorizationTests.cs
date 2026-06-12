using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Lab2.RezervacijeProstora.Tests
{
    public class AuthorizationTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;

        public AuthorizationTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            });
        }

        [Theory]
        [InlineData("/Lokacija")]
        [InlineData("/Lokacija/Search?q=Zagreb")]
        [InlineData("/ProstorZaProbu")]
        [InlineData("/ProstorZaProbu/Search?q=Studio")]
        public async Task PublicActions_ShouldBeAvailableWithoutLogin(string url)
        {
            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Theory]
        [InlineData("/Lokacija/Details/1")]
        [InlineData("/Lokacija/Create")]
        [InlineData("/Lokacija/Edit/1")]
        [InlineData("/Lokacija/Delete/1")]
        [InlineData("/ProstorZaProbu/Files/1")]
        [InlineData("/ProstorZaProbu/Edit/1")]
        public async Task ProtectedActions_ShouldRedirectAnonymousUsersToLogin(string url)
        {
            var response = await _client.GetAsync(url);

            response.StatusCode.Should().Be(HttpStatusCode.Redirect);
            response.Headers.Location?.OriginalString.Should().Contain("/Account/Login");
        }
    }
}
