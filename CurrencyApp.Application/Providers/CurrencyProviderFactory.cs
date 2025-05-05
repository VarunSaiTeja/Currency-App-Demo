using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace CurrencyApp.Application.Providers;

public enum CurrencyProviderType
{
    Frankfurter,
    OpenExchangeRates
}

[ExcludeFromCodeCoverage]
public class CurrencyProviderFactory(IServiceProvider services)
{
    public virtual ICurrencyProvider Get(CurrencyProviderType providerType = CurrencyProviderType.Frankfurter)
    {
        return services.GetRequiredKeyedService<ICurrencyProvider>(providerType)
            ?? throw new ArgumentException($"No provider found for type {providerType}");
    }
}
