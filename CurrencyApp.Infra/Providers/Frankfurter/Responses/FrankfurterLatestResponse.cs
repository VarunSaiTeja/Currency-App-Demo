using CurrencyApp.Application.Features.Currencies.Conversion;
using CurrencyApp.Application.Features.Currencies.ExchangeRates;

namespace CurrencyApp.Infra.Providers.Frankfurter.Responses;
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

    public ConversionResponse CreateConversionResponse(decimal amount)
    {
        return new ConversionResponse
        {
            Base = Base,
            Amount = amount,
            ConversionRates = [.. Rates.Select(rate => new ConversionRateInfo
            {
                Currency = rate.Key,
                Amount = Math.Round(rate.Value * amount, 2),
                ConversionRate = rate.Value
            })]
        };
    }
}
