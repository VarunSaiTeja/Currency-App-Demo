using CurrencyApp.Application.Features.Currencies.Conversion;
using CurrencyApp.Application.Features.Currencies.ExchangeRates;
using CurrencyApp.Application.Features.Currencies.HistoricRates;

namespace CurrencyApp.Application.Providers;

public interface ICurrencyProvider
{
    Task<ExchangeRatesResponse> ExchangeRates(ExchangeRatesRequest request);

    Task<ConversionResponse> Conversion(ConversionRequest request);

    Task<HistoricRatesResponse> HistoricRates(HistoricRatesRequest request);
}