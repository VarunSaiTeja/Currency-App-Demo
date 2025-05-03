using CurrencyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Providers;

public interface ICurrencyProvider
{
    Task<ExchangeRatesResponse> ExchangeRates(ExchangeRatesRequest request);

    Task<ConversionResponse> Conversion(ConversionRequest request);

    Task<HistoricRatesResponse> HistoricRates([FromQuery] HistoricRatesRequest request);
}