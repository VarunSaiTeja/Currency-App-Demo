using CurrencyApp.Application.Features.Currencies.HistoricRates;
using CurrencyApp.Application.Providers;
using Moq;
namespace CurrencyAPI.Tests.Features.Currencies.HistoricRates;
public class HistoricRatesHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Expected_Response()
    {
        // Arrange
        var request = new HistoricRatesRequest
        {
            Base = "USD",
            StartDate = new DateTime(2024, 1, 1),
            EndDate = new DateTime(2024, 1, 3),
            Page = 1,
            PageSize = 2
        };
        var expectedResponse = new HistoricRatesResponse
        {
            Base = "USD",
            StartDate = "2024-01-01",
            EndDate = "2024-01-03",
            HistoricRates = new List<HistoricRateDateSnapshot>
            {
                new HistoricRateDateSnapshot
                {
                    Date = "2024-01-01",
                    Rates = new List<HistoricRateDateSnapshot.RateInfo>
                    {
                        new HistoricRateDateSnapshot.RateInfo { Currency = "EUR", Rate = 0.9m }
                    }
                },
                new HistoricRateDateSnapshot
                {
                    Date = "2024-01-02",
                    Rates = new List<HistoricRateDateSnapshot.RateInfo>
                    {
                        new HistoricRateDateSnapshot.RateInfo { Currency = "EUR", Rate = 0.91m }
                    }
                }
            },
            TotalCount = 2,
            TotalPages = 1,
            CurrentPage = 1
        };

        var providerMock = new Mock<ICurrencyProvider>();
        providerMock.Setup(p => p.HistoricRates(request)).ReturnsAsync(expectedResponse);

        var factoryMock = new Mock<CurrencyProviderFactory>(Mock.Of<IServiceProvider>());
        factoryMock.Setup(f => f.Get(CurrencyProviderType.Frankfurter)).Returns(providerMock.Object);

        var handler = new HistoricRatesHandler(factoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.Base, result.Base);
        Assert.Equal(expectedResponse.StartDate, result.StartDate);
        Assert.Equal(expectedResponse.EndDate, result.EndDate);
        Assert.Equal(expectedResponse.HistoricRates.Count, result.HistoricRates.Count);
        Assert.Equal(expectedResponse.TotalCount, result.TotalCount);
        Assert.Equal(expectedResponse.TotalPages, result.TotalPages);
        Assert.Equal(expectedResponse.CurrentPage, result.CurrentPage);
        Assert.Equal(expectedResponse.HistoricRates[0].Date, result.HistoricRates[0].Date);
        Assert.Equal(expectedResponse.HistoricRates[0].Rates[0].Currency, result.HistoricRates[0].Rates[0].Currency);
        Assert.Equal(expectedResponse.HistoricRates[0].Rates[0].Rate, result.HistoricRates[0].Rates[0].Rate);
    }
}
