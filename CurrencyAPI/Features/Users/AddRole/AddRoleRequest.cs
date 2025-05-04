using CurrencyApp.Data.Entities;
using MediatR;

namespace CurrencyAPI.Features.Users.AddRole;

public class AddRoleRequest : IRequest<AddRoleResponse>
{
    public int UserId { get; set; }
    public UserRole Role { get; set; }
}