using Polly;
using Polly.Extensions.Http;

namespace CurrencyAPI.BuilderExtensions;

public class PollyExtensions
{
    public static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()  // Retry on 5xx and network failures
            .WaitAndRetryAsync(3, retryAttempt =>
                TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) // Exponential backoff (2, 4, 8 seconds)
            );
    }

    public static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .CircuitBreakerAsync(
                3, // Maximum 3 consecutive failures before circuit opens
                TimeSpan.FromMinutes(1) // Circuit stays open for 1 minute
            );
    }
}
