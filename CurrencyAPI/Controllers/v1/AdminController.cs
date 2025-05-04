using Asp.Versioning;
using CurrencyAPI.Features.Users.AddRole;
using CurrencyAPI.Features.Users.GetUser;
using CurrencyAPI.Features.Users.RemoveRole;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyAPI.Controllers.v1;

[ApiVersion(1)]
[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/v{v:apiVersion}/[controller]/[action]")]
public class AdminController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<GetUserResponse> GetUser([FromQuery] GetUserRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost]
    public async Task<AddRoleResponse> AddRole(AddRoleRequest request)
    {
        return await mediator.Send(request);
    }

    [HttpPost]
    public async Task<RemoveRoleResponse> RemoveRole(RemoveRoleRequest request)
    {
        return await mediator.Send(request);
    }
}