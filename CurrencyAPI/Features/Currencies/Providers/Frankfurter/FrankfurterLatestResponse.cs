using CurrencyAPI.Features.Currencies.ExchangeRates;
using CurrencyAPI.Features.Currencies.HistoricRates;

namespace CurrencyAPI.Features.Currencies.Providers.Frankfurter;
public class FrankfurterLatestResponse
{
    public decimal Amount { get; set; }
    public string Base { get; set; }
    public DateTime Date { get; set; }
    public Dictionary<string, decimal> Rates { get; set; }


    public static implicit operator ExchangeRatesResponse(FrankfurterLatestResponse response)
    {
        return new ExchangeRatesResponse
        {
            BaseCurrency = response.Base,
            Date = response.Date,
            ExchangeRates = [.. response.Rates.Select(rate => new ExchangeRateInfo
            {
                Currency = rate.Key,
                Rate = rate.Value
            })]
        };
    }
}

public class FrankfurterHistoricResponse
{
    public decimal Amount { get; set; }
    public string Base { get; set; }
    public string Start_Date { get; set; }
    public string End_Date { get; set; }

    /// Dictionary of dates with their respective rates
    public Dictionary<string, Dictionary<string, decimal>> Rates { get; set; }

    public static implicit operator HistoricRatesResponse(FrankfurterHistoricResponse response)
    {
        return new HistoricRatesResponse
        {
            Base = response.Base,
            StartDate = response.Start_Date,
            EndDate = response.End_Date,
            HistoricRates = [.. response.Rates.Select(rate => new HistoricRateDateSnapshot
            {
                Date = rate.Key,
                Rates = [.. rate.Value.Select(r => new HistoricRateDateSnapshot.RateInfo
                {
                    Currency = r.Key,
                    Rate = r.Value
                })]
            })]
        };
    }
}