using CurrencyApp.Application.Features.Currencies.Conversion;
using FluentValidation.TestHelper;
namespace CurrencyAPI.Tests.Features.Currencies.Conversion;
public class ConversionRequestValidatorTests
{
    private readonly ConversionRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Base_Is_Empty()
    {
        var model = new ConversionRequest { Base = "", Amount = 10, Symbols = ["USD"] };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Base);
    }

    [Fact]
    public void Should_Have_Error_When_Base_Is_Invalid_Format()
    {
        var model = new ConversionRequest { Base = "US", Amount = 10, Symbols = ["USD"] };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Base);
    }

    [Fact]
    public void Should_Have_Error_When_Amount_Is_Zero_Or_Negative()
    {
        var model = new ConversionRequest { Base = "USD", Amount = 0, Symbols = ["EUR"] };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Amount);
    }

    [Fact]
    public void Should_Have_Error_When_Symbols_Is_Null_Or_Empty()
    {
        var model1 = new ConversionRequest { Base = "USD", Amount = 10, Symbols = null };
        var result1 = _validator.TestValidate(model1);
        result1.ShouldHaveValidationErrorFor(x => x.Symbols);

        var model2 = new ConversionRequest { Base = "USD", Amount = 10, Symbols = new List<string>() };
        var result2 = _validator.TestValidate(model2);
        result2.ShouldHaveValidationErrorFor(x => x.Symbols);
    }

    [Fact]
    public void Should_Have_Error_When_Symbols_Contain_Disallowed_Currencies()
    {
        var model = new ConversionRequest { Base = "USD", Amount = 10, Symbols = ["TRY", "EUR"] };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Symbols);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Request()
    {
        var model = new ConversionRequest { Base = "USD", Amount = 100, Symbols = ["EUR", "GBP"] };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
