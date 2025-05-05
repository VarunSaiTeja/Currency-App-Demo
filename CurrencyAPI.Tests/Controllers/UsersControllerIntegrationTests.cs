using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using CurrencyApp.Application.Features.Users.Login;
using CurrencyApp.Application.Features.Users.RefreshAccessToken;
using CurrencyApp.Application.Features.Users.Register;
using Xunit;

namespace CurrencyAPI.Tests.Controllers;

public class UsersControllerIntegrationTests : IClassFixture<MockedWebApplicationFactory>
{
    private readonly MockedWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public UsersControllerIntegrationTests(MockedWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_ValidUser_Returns_Success()
    {
        var request = new RegisterUserRequest
        {
            Name = "TestUser",
            Email = $"testuser_{System.Guid.NewGuid()}@app.com",
            Password = "Test@1234"
        };
        var response = await _client.PostAsJsonAsync("/api/v1/Users/Register", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RegisterUserResponse>();
        Assert.NotNull(result);
        Assert.Equal(request.Email, result.Email);
    }

    [Fact]
    public async Task Register_InvalidUser_Returns_BadRequest()
    {
        var request = new RegisterUserRequest { Name = "", Email = "bademail", Password = "123" };
        var response = await _client.PostAsJsonAsync("/api/v1/Users/Register", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Login_ValidCredentials_Returns_Token()
    {
        var request = new LoginUserRequest { Email = "admin@app.com", Password = "Admin@123" };
        var response = await _client.PostAsJsonAsync("/api/v1/Users/Login", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginUserResponse>();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.AccessToken));
        Assert.False(string.IsNullOrEmpty(result.RefreshToken));
    }

    [Fact]
    public async Task Login_InvalidCredentials_Returns_BadRequest()
    {
        var request = new LoginUserRequest { Email = "admin@app.com", Password = "WrongPassword" };
        var response = await _client.PostAsJsonAsync("/api/v1/Users/Login", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task RefreshToken_ValidRequest_Returns_NewToken()
    {
        // First, login to get a refresh token
        var loginRequest = new LoginUserRequest { Email = "admin@app.com", Password = "Admin@123" };
        var loginResponse = await _client.PostAsJsonAsync("/api/v1/Users/Login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();
        var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginUserResponse>();
        var refreshRequest = new RefreshTokenRequest { RefreshToken = loginResult.RefreshToken };
        var response = await _client.PostAsJsonAsync("/api/v1/Users/RefreshToken", refreshRequest);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RefreshTokenResponse>();
        Assert.NotNull(result);
        Assert.False(string.IsNullOrEmpty(result.AccessToken));
    }

    [Fact]
    public async Task RefreshToken_InvalidToken_Returns_BadRequest()
    {
        var refreshRequest = new RefreshTokenRequest { RefreshToken = "invalidtoken" };
        var response = await _client.PostAsJsonAsync("/api/v1/Users/RefreshToken", refreshRequest);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
