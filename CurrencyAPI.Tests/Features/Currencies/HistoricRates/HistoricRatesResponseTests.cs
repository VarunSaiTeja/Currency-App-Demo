using CurrencyApp.Application.Features.Currencies.HistoricRates;

namespace CurrencyAPI.Tests.Features.Currencies.HistoricRates;

public class HistoricRatesResponseTests
{
    [Theory]
    [InlineData(1, 1, false)]
    [InlineData(1, 2, true)]
    [InlineData(2, 2, false)]
    [InlineData(2, 3, true)]
    public void HasNextPage_Returns_Correct_Value(int currentPage, int totalPages, bool expected)
    {
        var response = new HistoricRatesResponse
        {
            CurrentPage = currentPage,
            TotalPages = totalPages
        };
        Assert.Equal(expected, response.HasNextPage);
    }
}
