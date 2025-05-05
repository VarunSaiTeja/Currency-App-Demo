using CurrencyApp.Application.Features.Currencies.Conversion;
using CurrencyApp.Application.Features.Currencies.ExchangeRates;
using CurrencyApp.Application.Features.Currencies.HistoricRates;
using CurrencyApp.Application.Providers;
using CurrencyApp.Infra.Providers.Frankfurter.Responses;
using Microsoft.Extensions.Caching.Memory;

namespace CurrencyApp.Infra.Providers.Frankfurter;

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

        return frankResp.CreateConversionResponse(request.Amount);
    }

    public async Task<HistoricRatesResponse> HistoricRates(HistoricRatesRequest request)
    {
        var startDate = request.GetFormattedStartDate();
        var endDate = request.GetFormattedEndDate();

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

        return frankResp.ConvertToHistoricRatesResponse(request.PageSize, request.Page);
    }
}