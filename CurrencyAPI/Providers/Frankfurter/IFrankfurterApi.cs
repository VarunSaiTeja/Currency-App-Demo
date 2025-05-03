using Refit;

namespace CurrencyAPI.Providers.Frankfurter;

public interface IFrankfurterApi
{
    [Get("/latest")]
    Task<FrankfurterLatestResponse> GetExchangeRatesAsync(
        [AliasAs("base")] string baseCurrency,
        [Query(CollectionFormat.Csv)] params string[] symbols);

    [Get("/{startDate}..{endDate}")]
    Task<FrankfurterHistoricResponse> HistoricRates(
        [AliasAs("base")] string baseCurrency,
        string startDate, string endDate);
}