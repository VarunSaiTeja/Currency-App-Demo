using CurrencyApp.Application.Features.Currencies.HistoricRates;

namespace CurrencyApp.Infra.Providers.Frankfurter.Responses;

public class FrankfurterHistoricResponse
{
    public decimal Amount { get; set; }
    public string Base { get; set; }
    public string Start_Date { get; set; }
    public string End_Date { get; set; }

    /// Dictionary of dates with their respective rates
    public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }

    public virtual HistoricRatesResponse ConvertToHistoricRatesResponse(int pageSize, int currentPage)
    {
        var currentPageRates = Rates
            .Skip(pageSize * (currentPage - 1))
            .Take(pageSize)
            .ToDictionary(x => x.Key, x => x.Value);

        return new HistoricRatesResponse
        {
            Base = Base,
            StartDate = Start_Date,
            EndDate = End_Date,
            HistoricRates = [.. currentPageRates.Select(rate => new HistoricRateDateSnapshot
            {
                Date = rate.Key,
                Rates = [.. rate.Value.Select(r => new HistoricRateDateSnapshot.RateInfo
                {
                    Currency = r.Key,
                    Rate = r.Value
                })]
            })],
            TotalCount = Rates.Count,
            TotalPages = (int)Math.Ceiling((double)Rates.Count / pageSize),
            CurrentPage = currentPage
        };
    }
}