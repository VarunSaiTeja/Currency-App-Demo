using FluentValidation;

namespace CurrencyAPI.DTOs;

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

public class ExchangeRatesRequest
{
    public string Base { get; set; }
}

public class ExchangeRatesRequestValidator : AbstractValidator<ExchangeRatesRequest>
{
    public ExchangeRatesRequestValidator()
    {
        RuleFor(x => x.Base)
            .NotEmpty()
            .WithMessage("Base currency is required.")
            .Matches(@"^[A-Z]{3}$")
            .WithMessage("Base currency must be a valid 3-letter currency code.");
    }
}