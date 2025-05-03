using CurrencyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Providers.Frankfurter;

public class FrankfurterProvider(IFrankfurterApi api) : ICurrencyProvider
{
    public async Task<ConversionResponse> Conversion(ConversionRequest request)
    {
        return await api.GetExchangeRatesAsync(request.Base, [.. request.Symbols]);
    }

    public async Task<ExchangeRatesResponse> ExchangeRates(ExchangeRatesRequest request)
    {
        return await api.GetExchangeRatesAsync(request.Base);
    }

    public async Task<HistoricRatesResponse> HistoricRates([FromQuery] HistoricRatesRequest request)
    {
        var startDate = request.StartDate.ToString("yyyy-MM-dd");
        var endDate = request.EndDate.ToString("yyyy-MM-dd");

        var frankResp = await api.HistoricRates(request.Base, startDate, endDate);

        var pageRates = frankResp.Rates.Skip(request.PageSize * (request.Page - 1))
            .Take(request.PageSize).Select(rate => new HistoricRateDateSnapshot
            {
                Date = rate.Key,
                Rates = [.. rate.Value.Select(r => new HistoricRateDateSnapshot.RateInfo
                {
                    Currency = r.Key,
                    Rate = r.Value
                })]
            }).ToList();

        var resp = new HistoricRatesResponse
        {
            Base = frankResp.Base,
            StartDate = frankResp.Start_Date,
            EndDate = frankResp.End_Date,
            HistoricRates = pageRates,
            TotalCount = frankResp.Rates.Count,
            TotalPages = (int)Math.Ceiling((double)frankResp.Rates.Count / request.PageSize),
            CurrentPage = request.Page,
        };
        return resp;
    }
}