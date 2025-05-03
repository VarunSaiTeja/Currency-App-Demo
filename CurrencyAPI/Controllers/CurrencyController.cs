using CurrencyAPI.DTOs;
using CurrencyAPI.Providers;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CurrencyController(CurrencyProviderFactory factory) : ControllerBase
{
    [HttpGet]
    public async Task<ExchangeRatesResponse> ExchangeRates([FromQuery] ExchangeRatesRequest request)
    {
        var provider = factory.Get();
        return await provider.ExchangeRates(request);
    }

    [HttpGet]
    public async Task<ConversionResponse> Conversion([FromQuery] ConversionRequest request)
    {
        var provider = factory.Get();
        return await provider.Conversion(request);
    }

    [HttpGet]
    public async Task<HistoricRatesResponse> HistoricRates([FromQuery] HistoricRatesRequest request)
    {
        var provider = factory.Get(CurrencyProviderType.Frankfurter);
        return await provider.HistoricRates(request);
    }
}