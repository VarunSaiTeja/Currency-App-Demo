using Asp.Versioning;
using Asp.Versioning.Conventions;
using CurrencyAPI.Extensions;
using CurrencyAPI.Providers;
using CurrencyAPI.Providers.Frankfurter;
using FluentValidation;
using FluentValidation.AspNetCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new HeaderApiVersionReader("X-Api-Version"),
        new QueryStringApiVersionReader());
})
.AddMvc()
.AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerVersioning();
builder.Services.AddMemoryCache();
builder.Services.AddResponseCaching();

builder.Services.AddValidatorsFromAssembly(typeof(Program).Assembly);
builder.Services.AddFluentValidationAutoValidation();
builder.Services
    .AddRefitClient<IFrankfurterApi>()
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://api.frankfurter.dev/v1"))
    .AddPolicyHandler(PollyExtensions.GetRetryPolicy())
    .AddPolicyHandler(PollyExtensions.GetCircuitBreakerPolicy());

builder.Services.AddKeyedScoped<ICurrencyProvider, FrankfurterProvider>(CurrencyProviderType.Frankfurter);
builder.Services.AddScoped<CurrencyProviderFactory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var descriptions = app.DescribeApiVersions();
        foreach (var description in descriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseResponseCaching();
app.MapControllers();

app.Run();
