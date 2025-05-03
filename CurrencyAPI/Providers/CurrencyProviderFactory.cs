namespace CurrencyAPI.Providers;

public enum CurrencyProviderType
{
    Frankfurter,
    OpenExchangeRates
}

public class CurrencyProviderFactory(IServiceProvider services)
{
    public virtual ICurrencyProvider Get(CurrencyProviderType providerType = CurrencyProviderType.Frankfurter)
    {
        return services.GetKeyedService<ICurrencyProvider>(providerType)
            ?? throw new ArgumentException($"No provider found for type {providerType}");
    }
}
