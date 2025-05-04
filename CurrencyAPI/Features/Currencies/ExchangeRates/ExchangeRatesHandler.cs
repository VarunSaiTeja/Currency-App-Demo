using CurrencyAPI.Features.Currencies.Providers;
using MediatR;

namespace CurrencyAPI.Features.Currencies.ExchangeRates;

public class ExchangeRatesHandler(CurrencyProviderFactory factory) : IRequestHandler<ExchangeRatesRequest, ExchangeRatesResponse>
{
    public async Task<ExchangeRatesResponse> Handle(ExchangeRatesRequest request, CancellationToken cancellationToken)
    {
        return await factory.Get().ExchangeRates(request);
    }
}
