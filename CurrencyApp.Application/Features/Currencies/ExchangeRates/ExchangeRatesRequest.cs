using MediatR;

namespace CurrencyApp.Application.Features.Currencies.ExchangeRates;

public class ExchangeRatesRequest : IRequest<ExchangeRatesResponse>
{
    public string Base { get; set; }
}