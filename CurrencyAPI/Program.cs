using CurrencyAPI.Extensions;
using CurrencyAPI.Providers;
using CurrencyAPI.Providers.Frankfurter;
using FluentValidation;
using FluentValidation.AspNetCore;
using Refit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
