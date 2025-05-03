using FluentValidation;

namespace CurrencyAPI.DTOs;

public class HistoricRatesRequest
{
    public string Base { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }

    public ushort Page { get; set; } = 1;
    public ushort PageSize { get; set; } = 10;
}

public class HistoricRatesResponse
{
    public string Base { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<HistoricRateInfo> HistoricRates { get; set; }

    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
}

public class HistoricRateDateSnapshot
{
    public DateTime Date { get; set; }
    public List<HistoricRateInfo> Rates { get; set; }
}

public class HistoricRateInfo
{
    public string Currency { get; set; }
    public decimal Rate { get; set; }
}

public class HistoricRatesRequestValidator : AbstractValidator<HistoricRatesRequest>
{
    public HistoricRatesRequestValidator()
    {
        RuleFor(x => x.Base)
            .NotEmpty()
            .WithMessage("Base currency is required.")
            .Matches(@"^[A-Z]{3}$")
            .WithMessage("Base currency must be a valid 3-letter currency code.");

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .WithMessage("Start date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("Start date cannot be in the future.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to start date.");
    }
}