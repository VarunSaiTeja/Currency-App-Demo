using CurrencyApp.Application;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Polly.CircuitBreaker;
using System.Net;

namespace CurrencyAPI;

public class GlobalExHandler(ILogger<GlobalExHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails();
        switch (exception)
        {
            case DomainException domainException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Title = "Invalid-Operation";
                problemDetails.Detail = domainException.Message;
                problemDetails.Status = (int)HttpStatusCode.BadRequest;
                break;
            case BrokenCircuitException circuitException:
                httpContext.Response.StatusCode = (int)HttpStatusCode.ServiceUnavailable;
                problemDetails.Title = circuitException.Message;
                problemDetails.Detail = "The service is currently unavailable. Please try again later.";
                logger.LogError(circuitException, "Circuit breaker triggered: {Message}", circuitException.Message);
                break;
            case Exception ex:
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Title = "Internal Server Error";
                problemDetails.Detail = ex.Message + "\n" + ex.StackTrace;
                logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);
                break;
        }

        problemDetails.Instance = httpContext.Request.Path;
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        return true;
    }
}
