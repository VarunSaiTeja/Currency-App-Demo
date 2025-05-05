using Asp.Versioning;
using CurrencyApp.Application.Features.Users.Login;
using CurrencyApp.Application.Features.Users.RefreshAccessToken;
using CurrencyApp.Application.Features.Users.Register;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CurrencyAPI.Controllers.v1;
[ApiVersion(1)]
[ApiController]
[EnableRateLimiting("authentication")]
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
}