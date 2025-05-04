using MediatR;

namespace CurrencyAPI.Features.Currencies.ExchangeRates;

public class ExchangeRatesRequest : IRequest<ExchangeRatesResponse>
{
    public string Base { get; set; }
}