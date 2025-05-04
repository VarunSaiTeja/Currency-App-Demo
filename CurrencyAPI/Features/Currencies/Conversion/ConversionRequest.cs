using MediatR;

namespace CurrencyAPI.Features.Currencies.Conversion;

public class ConversionRequest : IRequest<ConversionResponse>
{
    public string Base { get; set; }
    public decimal Amount { get; set; }

    /// List of target currencies
    /// Currencies TRY, PLN, THB, and MXN are not allowed
    public List<string> Symbols { get; set; }
}