using CurrencyApp.Application.Features.Currencies.Conversion;
using CurrencyApp.Application.Features.Currencies.ExchangeRates;
using CurrencyApp.Infra.Providers.Frankfurter;
using CurrencyApp.Infra.Providers.Frankfurter.Responses;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace CurrencyAPI.Tests.Features.Currencies.Providers;
public class FrankfurterProviderTests
{
    [Fact]
    public void FrankfurterHistoricResponse_ConvertToHistoricRatesResponse_ReturnsCorrectPaging()
    {
        var rates = new Dictionary<string, Dictionary<string, decimal>>
        {
            { "2024-01-01", new Dictionary<string, decimal> { { "USD", 1.1m }, { "EUR", 1.0m } } },
            { "2024-01-02", new Dictionary<string, decimal> { { "USD", 1.2m }, { "EUR", 1.0m } } },
            { "2024-01-03", new Dictionary<string, decimal> { { "USD", 1.3m }, { "EUR", 1.0m } } }
        };
        var resp = new FrankfurterHistoricResponse
        {
            Amount = 1,
            Base = "EUR",
            Start_Date = "2024-01-01",
            End_Date = "2024-01-03",
            Rates = rates
        };
        var result = resp.ConvertToHistoricRatesResponse(2, 2);
        Assert.Equal("EUR", result.Base);
        Assert.Equal("2024-01-01", result.StartDate);
        Assert.Equal("2024-01-03", result.EndDate);
        Assert.Single(result.HistoricRates);
        Assert.Equal(3, result.TotalCount);
        Assert.Equal(2, result.TotalPages);
        Assert.Equal(2, result.CurrentPage);
    }

    [Fact]
    public void FrankfurterLatestResponse_ImplicitOperator_ExchangeRatesResponse_Works()
    {
        var resp = new FrankfurterLatestResponse
        {
            Amount = 1,
            Base = "USD",
            Date = new DateTime(2024, 1, 1),
            Rates = new Dictionary<string, decimal> { { "EUR", 0.9m }, { "GBP", 0.8m } }
        };
        ExchangeRatesResponse result = resp;
        Assert.Equal("USD", result.BaseCurrency);
        Assert.Equal(new DateTime(2024, 1, 1), result.Date);
        Assert.Equal(2, result.ExchangeRates.Count);
    }

    [Fact]
    public void FrankfurterLatestResponse_CreateConversionResponse_Works()
    {
        var resp = new FrankfurterLatestResponse
        {
            Amount = 1,
            Base = "USD",
            Date = DateTime.Now,
            Rates = new Dictionary<string, decimal> { { "EUR", 0.9m }, { "GBP", 0.8m } }
        };
        var result = resp.CreateConversionResponse(100);
        Assert.Equal("USD", result.Base);
        Assert.Equal(100, result.Amount);
        Assert.Equal(2, result.ConversionRates.Count);
        Assert.Contains(result.ConversionRates, r => r.Currency == "EUR" && r.Amount == 90);
        Assert.Contains(result.ConversionRates, r => r.Currency == "GBP" && r.Amount == 80);
    }

    [Fact]
    public async Task FrankfurterProvider_Conversion_UsesCacheAndReturnsConversionResponse()
    {
        var mockApi = new Mock<IFrankfurterApi>();
        var memoryCache = new MemoryCache(new MemoryCacheOptions());
        var provider = new FrankfurterProvider(mockApi.Object, memoryCache);
        var request = new ConversionRequest { Base = "USD", Symbols = ["EUR"], Amount = 10 };
        var frankResp = new FrankfurterLatestResponse
        {
            Amount = 1,
            Base = "USD",
            Date = DateTime.Now,
            Rates = new Dictionary<string, decimal> { { "EUR", 2.0m } }
        };
        mockApi.Setup(x => x.GetExchangeRatesAsync("USD", It.IsAny<List<string>>()))
            .ReturnsAsync(frankResp);
        var result1 = await provider.Conversion(request);
        var result2 = await provider.Conversion(request);
        Assert.Equal(20, result1.ConversionRates[0].Amount);
        Assert.Equal(20, result2.ConversionRates[0].Amount);
        mockApi.Verify(x => x.GetExchangeRatesAsync("USD", It.IsAny<List<string>>()), Times.Once);
    }
}
