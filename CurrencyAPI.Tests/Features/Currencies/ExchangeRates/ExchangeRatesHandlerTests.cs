using CurrencyApp.Application.Features.Currencies.ExchangeRates;
using CurrencyApp.Application.Providers;
using Moq;
namespace CurrencyAPI.Tests.Features.Currencies.ExchangeRates;
public class ExchangeRatesHandlerTests
{
    [Fact]
    public async Task Handle_Returns_Expected_Response()
    {
        // Arrange
        var request = new ExchangeRatesRequest { Base = "USD" };
        var expectedResponse = new ExchangeRatesResponse
        {
            BaseCurrency = "USD",
            Date = new DateTime(2024, 1, 1),
            ExchangeRates = [
                new ExchangeRateInfo { Currency = "EUR", Rate = 0.9m },
                new ExchangeRateInfo { Currency = "JPY", Rate = 110m }
            ]
        };

        var providerMock = new Mock<ICurrencyProvider>();
        providerMock.Setup(p => p.ExchangeRates(request)).ReturnsAsync(expectedResponse);

        var factoryMock = new Mock<CurrencyProviderFactory>(Mock.Of<IServiceProvider>());
        factoryMock.Setup(f => f.Get(CurrencyProviderType.Frankfurter)).Returns(providerMock.Object);

        var handler = new ExchangeRatesHandler(factoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse.BaseCurrency, result.BaseCurrency);
        Assert.Equal(expectedResponse.Date, result.Date);
        Assert.Equal(expectedResponse.ExchangeRates.Count, result.ExchangeRates.Count);
        Assert.Equal(expectedResponse.ExchangeRates[0].Currency, result.ExchangeRates[0].Currency);
        Assert.Equal(expectedResponse.ExchangeRates[0].Rate, result.ExchangeRates[0].Rate);
    }
}
