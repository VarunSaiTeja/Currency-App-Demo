using CurrencyAPI.Providers.Frankfurter;

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
        return providerType switch
        {
            CurrencyProviderType.Frankfurter => services.GetService<FrankfurterProvider>(),
            CurrencyProviderType.OpenExchangeRates => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }
}
