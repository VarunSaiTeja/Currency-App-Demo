using CurrencyAPI.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyAPI.Providers.Frankfurter;

public class FrankfurterProvider(IFrankfurterApi api, IMemoryCache cache) : ICurrencyProvider
{
    public async Task<ExchangeRatesResponse> ExchangeRates(ExchangeRatesRequest request)
    {
        return await api.GetExchangeRatesAsync(request.Base);
    }

    public async Task<ConversionResponse> Conversion(ConversionRequest request)
    {
        var cacheKey = $"Conversion_{request.Base}_{string.Join(",", request.Symbols)}";
        if (!cache.TryGetValue(cacheKey, out FrankfurterLatestResponse frankResp))
        {
            frankResp = await api.GetExchangeRatesAsync(request.Base, request.Symbols);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            };
            cache.Set(cacheKey, frankResp, cacheEntryOptions);
        }

        return new ConversionResponse
        {
            Base = frankResp.Base,
            Amount = request.Amount,
            ConversionRates = [.. frankResp.Rates.Select(rate => new ConversionRateInfo
            {
                Currency = rate.Key,
                Amount = Math.Round(rate.Value * request.Amount, 2),
                ConversionRate = rate.Value
            })]
        };
    }

    public async Task<HistoricRatesResponse> HistoricRates([FromQuery] HistoricRatesRequest request)
    {
        var startDate = request.StartDate.ToString("yyyy-MM-dd");
        var endDate = request.EndDate.ToString("yyyy-MM-dd");

        var cacheKey = $"HistoricRates_{request.Base}_{startDate}_{endDate}_{request.Page}_{request.PageSize}";
        if (!cache.TryGetValue(cacheKey, out FrankfurterHistoricResponse frankResp))
        {
            frankResp = await api.HistoricRates(request.Base, startDate, endDate);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            };
            cache.Set(cacheKey, frankResp, cacheEntryOptions);
        }

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