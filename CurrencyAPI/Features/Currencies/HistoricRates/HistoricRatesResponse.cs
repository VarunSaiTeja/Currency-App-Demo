namespace CurrencyAPI.Features.Currencies.HistoricRates;

public class HistoricRatesResponse
{
    public string Base { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
    public List<HistoricRateDateSnapshot> HistoricRates { get; set; }

    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public int CurrentPage { get; set; } = 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}

public class HistoricRateDateSnapshot
{
    public string Date { get; set; }
    public List<RateInfo> Rates { get; set; }

    public class RateInfo
    {
        public string Currency { get; set; }
        public decimal Rate { get; set; }
    }
}