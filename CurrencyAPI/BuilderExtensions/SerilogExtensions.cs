using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace CurrencyAPI.BuilderExtensions;

public static class SerilogExtensions
{
    public static LoggerConfiguration WithClientId(
        this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        ArgumentNullException.ThrowIfNull(enrichmentConfiguration);
        return enrichmentConfiguration.With<ClientIdEnricher>();
    }
}

public class ClientIdEnricher : ILogEventEnricher
{
    private const string ClientIdPropertyName = "ClientId";
    private const string ClientIdItemKey = "Serilog_ClientId";

    private readonly IHttpContextAccessor _contextAccessor;

    public ClientIdEnricher() : this(new HttpContextAccessor())
    {
    }

    internal ClientIdEnricher(IHttpContextAccessor contextAccessor)
    {
        _contextAccessor = contextAccessor;
    }

    /// <inheritdoc/>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var httpContext = _contextAccessor.HttpContext;
        if (httpContext == null || httpContext.User.Identity.IsAuthenticated is false)
        {
            return;
        }

        var clientId = httpContext.User.Identity.Name;

        if (httpContext.Items[ClientIdItemKey] is LogEventProperty logEventProperty)
        {
            if (!((ScalarValue)logEventProperty.Value).Value.ToString()!.Equals(clientId))
            {
                logEventProperty = new LogEventProperty(ClientIdPropertyName, new ScalarValue(clientId));
            }

            logEvent.AddPropertyIfAbsent(logEventProperty);
            return;
        }

        LogEventProperty clientIdProperty = new(ClientIdPropertyName, new ScalarValue(clientId));
        httpContext.Items.Add(ClientIdItemKey, clientIdProperty);
        logEvent.AddPropertyIfAbsent(clientIdProperty);
    }
}