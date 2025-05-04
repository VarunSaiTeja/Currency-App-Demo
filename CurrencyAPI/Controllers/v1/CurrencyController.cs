using Asp.Versioning;
using CurrencyAPI.DAL.Entities;
using CurrencyAPI.Features.Currencies.Conversion;
using CurrencyAPI.Features.Currencies.ExchangeRates;
using CurrencyAPI.Features.Currencies.HistoricRates;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CurrencyAPI.Controllers.v1;
[ApiVersion(1)]
[ApiController]
[Route("api/v{v:apiVersion}/[controller]/[action]")]
public class CurrencyController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Get the latest exchange rates for a given base currency.
    /// Response is cached for 5 minutes vary by query key - base.
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    [EnableRateLimiting("ip")]
    [ResponseCache(Duration = 60 * 5, VaryByQueryKeys = new[] { "base" })]
    public async Task<ExchangeRatesResponse> ExchangeRates([FromQuery] ExchangeRatesRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpGet]
    [EnableRateLimiting("user")]
    [Authorize(Roles = $"{nameof(UserRole.Customer)}")]
    public async Task<ConversionResponse> Conversion([FromQuery] ConversionRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpGet]
    [EnableRateLimiting("user")]
    [Authorize(Roles = $"{nameof(UserRole.Analyst)}")]
    public async Task<HistoricRatesResponse> HistoricRates([FromQuery] HistoricRatesRequest request)
    {
        return await mediator.Send(request);
    }
}