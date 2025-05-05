using FluentValidation;

namespace CurrencyApp.Application.Features.Currencies.ExchangeRates;

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