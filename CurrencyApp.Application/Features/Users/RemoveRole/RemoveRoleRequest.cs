using CurrencyApp.Data.Entities;
using MediatR;

namespace CurrencyApp.Application.Features.Users.RemoveRole;

public class RemoveRoleRequest : IRequest<RemoveRoleResponse>
{
    public int UserId { get; set; }
    public UserRole Role { get; set; }
}