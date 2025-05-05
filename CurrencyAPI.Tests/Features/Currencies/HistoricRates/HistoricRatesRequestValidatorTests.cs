using CurrencyApp.Application.Features.Currencies.HistoricRates;
using FluentValidation.TestHelper;
namespace CurrencyAPI.Tests.Features.Currencies.HistoricRates;
public class HistoricRatesRequestValidatorTests
{
    private readonly HistoricRatesRequestValidator _validator = new();

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("US")]
    [InlineData("USDE")]
    [InlineData("usd")]
    [InlineData("123")]
    public void Should_Have_Error_When_Base_Is_Invalid(string baseValue)
    {
        var model = new HistoricRatesRequest
        {
            Base = baseValue,
            StartDate = DateTime.UtcNow.AddDays(-2),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Base);
    }

    [Fact]
    public void Should_Have_Error_When_StartDate_Is_In_The_Future()
    {
        var model = new HistoricRatesRequest
        {
            Base = "USD",
            StartDate = DateTime.UtcNow.AddDays(1),
            EndDate = DateTime.UtcNow.AddDays(2)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.StartDate);
    }

    [Fact]
    public void Should_Have_Error_When_EndDate_Is_Before_StartDate()
    {
        var model = new HistoricRatesRequest
        {
            Base = "USD",
            StartDate = DateTime.UtcNow.AddDays(-1),
            EndDate = DateTime.UtcNow.AddDays(-2)
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.EndDate);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Request()
    {
        var model = new HistoricRatesRequest
        {
            Base = "USD",
            StartDate = DateTime.UtcNow.AddDays(-2),
            EndDate = DateTime.UtcNow.AddDays(-1)
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
