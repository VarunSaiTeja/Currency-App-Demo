using FluentValidation;
namespace CurrencyApp.Application.Features.Currencies.Conversion;

public class ConversionRequestValidator : AbstractValidator<ConversionRequest>
{
    private static readonly HashSet<string> DisallowedCurrencies = ["TRY", "PLN", "THB", "MXN"];
    public ConversionRequestValidator()
    {
        RuleFor(x => x.Base)
            .NotEmpty()
            .WithMessage("Base currency is required.")
            .Matches(@"^[A-Z]{3}$")
            .WithMessage("Base currency must be a valid 3-letter currency code.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than zero.");

        RuleFor(x => x.Symbols)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .NotEmpty()
            .WithMessage("At least one target currency is required.")
            .Must(symbols => symbols.All(symbol => !DisallowedCurrencies.Contains(symbol)))
            .WithMessage("Currencies TRY, PLN, THB, and MXN are not allowed.");
    }
}