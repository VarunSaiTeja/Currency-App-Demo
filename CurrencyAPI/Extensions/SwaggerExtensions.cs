using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CurrencyAPI.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddSwaggerVersioning(this IServiceCollection services)
    {
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerApiDescriptor>();
        return services;
    }
}

public class ConfigureSwaggerApiDescriptor(IApiVersionDescriptionProvider _provider) : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }
    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var info = new OpenApiInfo
        {
            Title = $"Currency API {description.ApiVersion}",
            Version = description.ApiVersion.ToString()
        };
        return info;
    }
}