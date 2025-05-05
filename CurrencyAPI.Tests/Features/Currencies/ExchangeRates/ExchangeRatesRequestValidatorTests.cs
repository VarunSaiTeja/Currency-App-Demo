using CurrencyApp.Application.Features.Currencies.ExchangeRates;
using FluentValidation.TestHelper;
namespace CurrencyAPI.Tests.Features.Currencies.ExchangeRates;
public class ExchangeRatesRequestValidatorTests
{
    private readonly ExchangeRatesRequestValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("US")]
    [InlineData("USDE")]
    [InlineData("usd")]
    [InlineData("123")]
    public void Should_Have_Error_When_Base_Is_Invalid(string baseValue)
    {
        var model = new ExchangeRatesRequest { Base = baseValue };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Base);
    }

    [Theory]
    [InlineData("USD")]
    [InlineData("EUR")]
    [InlineData("JPY")]
    public void Should_Not_Have_Error_When_Base_Is_Valid(string baseValue)
    {
        var model = new ExchangeRatesRequest { Base = baseValue };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.Base);
    }
}
