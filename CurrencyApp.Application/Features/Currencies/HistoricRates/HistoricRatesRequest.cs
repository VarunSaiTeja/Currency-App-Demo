using MediatR;

namespace CurrencyApp.Application.Features.Currencies.HistoricRates;

public class HistoricRatesRequest : IRequest<HistoricRatesResponse>
{
    public string Base { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ushort Page { get; set; } = 1;
    public ushort PageSize { get; set; } = 10;

    public string GetFormattedStartDate() => StartDate.ToString("yyyy-MM-dd");
    public string GetFormattedEndDate() => EndDate.ToString("yyyy-MM-dd");
}