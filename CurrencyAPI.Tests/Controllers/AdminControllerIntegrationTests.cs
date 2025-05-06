using CurrencyAPI.Tests.Setup;
using CurrencyApp.Application.Features.Users.AddRole;
using CurrencyApp.Application.Features.Users.GetUser;
using CurrencyApp.Application.Features.Users.RemoveRole;
using System.Net;
using System.Net.Http.Json;

namespace CurrencyAPI.Tests.Controllers;

public class AdminControllerIntegrationTests : IClassFixture<DefaultWebApplicationFactory>
{
    private readonly DefaultWebApplicationFactory _factory;
    private readonly HttpClient _client;

    public AdminControllerIntegrationTests(DefaultWebApplicationFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUser_Requires_AdminRole_Returns_Forbidden_If_Unauthenticated()
    {
        var response = await _client.GetAsync("/api/v1/Admin/GetUser?UserId=1");
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetUser_With_AdminRole_Returns_Success()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Test-Role", "Admin");
        var response = await client.GetAsync("/api/v1/Admin/GetUser?Email=varun@app.com");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<GetUserResponse>();
        Assert.NotNull(result);
        Assert.Equal(2, result.Id);
    }

    [Fact]
    public async Task AddRole_Requires_AdminRole_Returns_Forbidden_If_Unauthenticated()
    {
        var request = new AddRoleRequest { UserId = 2, Role = CurrencyApp.Data.Entities.UserRole.Analyst };
        var response = await _client.PostAsJsonAsync("/api/v1/Admin/AddRole", request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task AddRole_With_AdminRole_Returns_Success()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Test-Role", "Admin");
        var request = new AddRoleRequest { UserId = 2, Role = CurrencyApp.Data.Entities.UserRole.Analyst };
        var response = await client.PostAsJsonAsync("/api/v1/Admin/AddRole", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<AddRoleResponse>();
        Assert.NotNull(result);
        Assert.Equal(2, result.UserId);
        Assert.Contains(CurrencyApp.Data.Entities.UserRole.Analyst, result.Roles);
    }

    [Fact]
    public async Task RemoveRole_Requires_AdminRole_Returns_Forbidden_If_Unauthenticated()
    {
        var request = new RemoveRoleRequest { UserId = 2, Role = CurrencyApp.Data.Entities.UserRole.Analyst };
        var response = await _client.PostAsJsonAsync("/api/v1/Admin/RemoveRole", request);
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task RemoveRole_With_AdminRole_Returns_Success()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("Test-Role", "Admin");
        var request = new RemoveRoleRequest { UserId = 3, Role = CurrencyApp.Data.Entities.UserRole.Analyst };
        var response = await client.PostAsJsonAsync("/api/v1/Admin/RemoveRole", request);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RemoveRoleResponse>();
        Assert.NotNull(result);
        Assert.Equal(3, result.UserId);
        Assert.DoesNotContain(CurrencyApp.Data.Entities.UserRole.Analyst, result.Roles);
    }
}
