using CurrencyAPI.Features.Currencies.Providers;
using MediatR;

namespace CurrencyAPI.Features.Currencies.HistoricRates;

public class HistoricRatesHandler(CurrencyProviderFactory factory) : IRequestHandler<HistoricRatesRequest, HistoricRatesResponse>
{
    public async Task<HistoricRatesResponse> Handle(HistoricRatesRequest request, CancellationToken cancellationToken)
    {
        return await factory.Get().HistoricRates(request);
    }
}