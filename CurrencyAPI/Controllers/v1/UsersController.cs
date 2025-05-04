using Asp.Versioning;
using CurrencyAPI.Features.Users.AddRole;
using CurrencyAPI.Features.Users.GetRoles;
using CurrencyAPI.Features.Users.Login;
using CurrencyAPI.Features.Users.RefreshAccessToken;
using CurrencyAPI.Features.Users.Register;
using CurrencyAPI.Features.Users.RemoveRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Controllers.v1;
[ApiVersion(1)]
[ApiController]
[Route("api/v{v:apiVersion}/[controller]/[action]")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<RegisterUserResponse> Register(RegisterUserRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost]
    public async Task<LoginUserResponse> Login(LoginUserRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost]
    public async Task<RefreshTokenResponse> RefreshToken(RefreshTokenRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<GetUserResponse> GetUser([FromQuery]GetUserRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<AddRoleResponse> AddRole(AddRoleRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<RemoveRoleResponse> RemoveRole(RemoveRoleRequest request)
    {
        return await mediator.Send(request);
    }
}
