using CurrencyAPI.Tests.Setup;
using CurrencyApp.Application.Features.Currencies.Conversion;
using CurrencyApp.Application.Features.Currencies.ExchangeRates;
using CurrencyApp.Application.Features.Currencies.HistoricRates;
using System.Net;
using System.Net.Http.Json;

namespace CurrencyAPI.Tests.Controllers;

public class CurrencyControllerRateLimitingTests
{
    [Fact]
    public async Task Conversion_RateLimiting_Returns_TooManyRequests()
    {
        using var factory = new RateLimitingWebApplicationFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Test-Role", "Customer");
        var request = new ConversionRequest { Base = "USD", Amount = 10, Symbols = new List<string> { "EUR" } };
        HttpResponseMessage lastResponse = null;
        for (int i = 0; i < 21; i++)
        {
            lastResponse = await client.GetAsync($"/api/v1/Currency/Conversion?base={request.Base}&amount={request.Amount}&symbols={string.Join(",", request.Symbols)}");
        }
        Assert.Equal(HttpStatusCode.TooManyRequests, lastResponse.StatusCode);
    }

    [Fact]
    public async Task HistoricRates_RateLimiting_Returns_TooManyRequests()
    {
        using var factory = new RateLimitingWebApplicationFactory();
        using var client = factory.CreateClient();
        client.DefaultRequestHeaders.Add("Test-Role", "Analyst");
        var request = new HistoricRatesRequest {
            Base = "USD",
            StartDate = DateTime.UtcNow.AddDays(-10),
            EndDate = DateTime.UtcNow
        };
        HttpResponseMessage lastResponse = null;
        for (int i = 0; i < 21; i++)
        {
            lastResponse = await client.GetAsync($"/api/v1/Currency/HistoricRates?base={request.Base}&startDate={request.StartDate:yyyy-MM-dd}&endDate={request.EndDate:yyyy-MM-dd}");
        }
        Assert.Equal(HttpStatusCode.TooManyRequests, lastResponse.StatusCode);
    }
}
