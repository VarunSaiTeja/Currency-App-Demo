namespace CurrencyAPI.Features.Currencies.Conversion;

public class ConversionResponse
{
    public string Base { get; set; }
    public decimal Amount { get; set; }
    public List<ConversionRateInfo> ConversionRates { get; set; }
}

public class ConversionRateInfo
{
    public string Currency { get; set; }
    public decimal ConversionRate { get; set; }
    public decimal Amount { get; set; }
}