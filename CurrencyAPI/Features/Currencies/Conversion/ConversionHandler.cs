using CurrencyAPI.Features.Currencies.Providers;
using MediatR;

namespace CurrencyAPI.Features.Currencies.Conversion;

public class ConversionHandler(CurrencyProviderFactory factory) : IRequestHandler<ConversionRequest, ConversionResponse>
{
    public async Task<ConversionResponse> Handle(ConversionRequest request, CancellationToken cancellationToken)
    {
        return await factory.Get().Conversion(request);
    }
}