using CurrencyAPI.Tests.Setup;
using CurrencyApp.Application.Features.Users.Login;
using CurrencyApp.Application.Features.Users.RefreshAccessToken;
using CurrencyApp.Application.Features.Users.Register;
using System.Net;
using System.Net.Http.Json;

namespace CurrencyAPI.Tests.Controllers;

public class UsersControllerRateLimitingTests
{
    [Fact]
    public async Task Register_RateLimiting_Returns_TooManyRequests()
    {
        using var factory = new RateLimitingWebApplicationFactory();
        using var client = factory.CreateClient();
        var request = new RegisterUserRequest
        {
            Name = "RateLimitUser",
            Email = $"ratelimit_{System.Guid.NewGuid()}@app.com",
            Password = "Test@1234"
        };
        HttpResponseMessage lastResponse = null;
        for (int i = 0; i < 6; i++)
        {
            lastResponse = await client.PostAsJsonAsync("/api/v1/Users/Register", request);
        }
        Assert.Equal(HttpStatusCode.TooManyRequests, lastResponse.StatusCode);
    }

    [Fact]
    public async Task Login_RateLimiting_Returns_TooManyRequests()
    {
        using var factory = new RateLimitingWebApplicationFactory();
        using var client = factory.CreateClient();
        var request = new LoginUserRequest { Email = "admin@app.com", Password = "Admin@123" };
        HttpResponseMessage lastResponse = null;
        for (int i = 0; i < 6; i++)
        {
            lastResponse = await client.PostAsJsonAsync("/api/v1/Users/Login", request);
        }
        Assert.Equal(HttpStatusCode.TooManyRequests, lastResponse.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_RateLimiting_Returns_TooManyRequests()
    {
        using var factory = new RateLimitingWebApplicationFactory();
        using var client = factory.CreateClient();
        // First, login to get a refresh token
        var loginRequest = new LoginUserRequest { Email = "admin@app.com", Password = "Admin@123" };
        var loginResponse = await client.PostAsJsonAsync("/api/v1/Users/Login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginUserResponse>();
        var refreshRequest = new RefreshTokenRequest { RefreshToken = loginResult.RefreshToken };
        HttpResponseMessage lastResponse = null;
        for (int i = 0; i < 6; i++)
        {
            lastResponse = await client.PostAsJsonAsync("/api/v1/Users/RefreshToken", refreshRequest);
        }
        Assert.Equal(HttpStatusCode.TooManyRequests, lastResponse.StatusCode);
    }
}
