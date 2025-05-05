using CurrencyApp.Application.Providers;
using MediatR;

namespace CurrencyApp.Application.Features.Currencies.Conversion;

public class ConversionHandler(CurrencyProviderFactory factory) : IRequestHandler<ConversionRequest, ConversionResponse>
{
    public async Task<ConversionResponse> Handle(ConversionRequest request, CancellationToken cancellationToken)
    {
        return await factory.Get().Conversion(request);
    }
}