using CurrencyApp.Application.Providers;
using MediatR;

namespace CurrencyApp.Application.Features.Currencies.HistoricRates;

public class HistoricRatesHandler(CurrencyProviderFactory factory) : IRequestHandler<HistoricRatesRequest, HistoricRatesResponse>
{
    public async Task<HistoricRatesResponse> Handle(HistoricRatesRequest request, CancellationToken cancellationToken)
    {
        return await factory.Get().HistoricRates(request);
    }
}