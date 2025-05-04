namespace CurrencyAPI.Features.Currencies.ExchangeRates;

public class ExchangeRatesResponse
{
    public string BaseCurrency { get; set; }
    public DateTime Date { get; set; }
    public List<ExchangeRateInfo> ExchangeRates { get; set; }
}

public class ExchangeRateInfo
{
    public string Currency { get; set; }
    public decimal Rate { get; set; }
}
