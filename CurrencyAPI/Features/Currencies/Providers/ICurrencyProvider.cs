using CurrencyAPI.Features.Currencies.Conversion;
using CurrencyAPI.Features.Currencies.ExchangeRates;
using CurrencyAPI.Features.Currencies.HistoricRates;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Features.Currencies.Providers;

public interface ICurrencyProvider
{
    Task<ExchangeRatesResponse> ExchangeRates(ExchangeRatesRequest request);

    Task<ConversionResponse> Conversion(ConversionRequest request);

    Task<HistoricRatesResponse> HistoricRates([FromQuery] HistoricRatesRequest request);
}