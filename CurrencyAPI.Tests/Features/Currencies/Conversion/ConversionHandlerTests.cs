using CurrencyApp.Application.Features.Currencies.Conversion;
using CurrencyApp.Application.Providers;
using Moq;
namespace CurrencyAPI.Tests.Features.Currencies.Conversion;
public class ConversionHandlerTests
{
    [Fact]
    public async Task Handle_Calls_Provider_And_Returns_Response()
    {
        // Arrange
        var request = new ConversionRequest { Base = "USD", Amount = 10, Symbols = ["EUR"] };
        var expectedResponse = new ConversionResponse();
        var providerMock = new Mock<ICurrencyProvider>();
        providerMock.Setup(p => p.Conversion(request)).ReturnsAsync(expectedResponse);
        var factoryMock = new Mock<CurrencyProviderFactory>(Mock.Of<IServiceProvider>());
        factoryMock.Setup(f => f.Get(CurrencyProviderType.Frankfurter)).Returns(providerMock.Object);
        var handler = new ConversionHandler(factoryMock.Object);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(expectedResponse, result);
        providerMock.Verify(p => p.Conversion(request), Times.Once);
    }
}
