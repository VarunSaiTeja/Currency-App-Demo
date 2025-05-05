using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CurrencyApp.Application.Features.Currencies.Conversion;
using CurrencyApp.Application.Features.Currencies.ExchangeRates;
using CurrencyApp.Application.Features.Currencies.HistoricRates;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CurrencyAPI.Tests.Controllers;

public class CurrencyControllerIntegrationTests : IClassFixture<MockedWebApplicationFactory>
{
    private readonly MockedWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public CurrencyControllerIntegrationTests(MockedWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task ExchangeRates_Allows_Anonymous_Returns_Success()
    {
        var response = await _client.GetAsync("/api/v1/Currency/ExchangeRates?Base=USD");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ExchangeRatesResponse>();
        Assert.NotNull(result);
        Assert.Equal("USD", result.BaseCurrency);
    }

    [Fact]
    public async Task Conversion_Requires_CustomerRole_Returns_Forbidden_If_Unauthenticated()
    {
        var response = await _client.GetAsync("/api/v1/Currency/Conversion?Base=USD&Amount=10&Symbols=EUR");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Conversion_With_CustomerRole_Returns_Success()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Test-Role", "Customer");
        var response = await client.GetAsync("/api/v1/Currency/Conversion?Base=USD&Amount=10&Symbols=EUR");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<ConversionResponse>();
        Assert.NotNull(result);
        Assert.Equal("USD", result.Base);
        Assert.Equal(10, result.Amount);
    }

    [Fact]
    public async Task HistoricRates_Requires_AnalystRole_Returns_Forbidden_If_Unauthenticated()
    {
        var response = await _client.GetAsync($"/api/v1/Currency/HistoricRates?Base=USD&StartDate=2024-01-01&EndDate=2024-01-03");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task HistoricRates_With_AnalystRole_Returns_Success()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Test-Role", "Analyst");
        var response = await client.GetAsync($"/api/v1/Currency/HistoricRates?Base=USD&StartDate=2024-01-01&EndDate=2024-01-03");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<HistoricRatesResponse>();
        Assert.NotNull(result);
        Assert.Equal("USD", result.Base);
    }
}
