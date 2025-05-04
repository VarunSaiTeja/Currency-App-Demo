using Refit;

namespace CurrencyAPI.Features.Currencies.Providers.Frankfurter;

public interface IFrankfurterApi
{
    [Get("/latest")]
    Task<FrankfurterLatestResponse> GetExchangeRatesAsync(
        [AliasAs("base")] string baseCurrency);

    [Get("/latest")]
    Task<FrankfurterLatestResponse> GetExchangeRatesAsync(
        [AliasAs("base")] string baseCurrency,
        [Query(CollectionFormat.Csv)] List<string> symbols);

    [Get("/{startDate}..{endDate}")]
    Task<FrankfurterHistoricResponse> HistoricRates(
        [AliasAs("base")] string baseCurrency,
        string startDate, string endDate);
}