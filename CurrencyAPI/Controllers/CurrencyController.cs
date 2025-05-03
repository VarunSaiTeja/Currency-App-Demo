using CurrencyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class CurrencyController : ControllerBase
{
    [HttpGet]
    public async Task<ExchangeRatesResponse> ExchangeRates([FromQuery] ExchangeRatesRequest request)
    {
        return null;
    }

    [HttpGet]
    public async Task<ConversionResponse> Conversion([FromQuery] ConversionRequest request)
    {
        return null;
    }

    [HttpGet]
    public async Task<HistoricRatesResponse> HistoricRates([FromQuery] HistoricRatesRequest request)
    {
        return null;
    }
}