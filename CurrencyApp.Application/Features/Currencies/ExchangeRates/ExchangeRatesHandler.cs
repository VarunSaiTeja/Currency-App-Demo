using CurrencyApp.Application.Providers;
using MediatR;

namespace CurrencyApp.Application.Features.Currencies.ExchangeRates;

public class ExchangeRatesHandler(CurrencyProviderFactory factory) : IRequestHandler<ExchangeRatesRequest, ExchangeRatesResponse>
{
    public async Task<ExchangeRatesResponse> Handle(ExchangeRatesRequest request, CancellationToken cancellationToken)
    {
        return await factory.Get().ExchangeRates(request);
    }
}
