using CurrencyAPI.DAL.Entities;
using MediatR;

namespace CurrencyAPI.Features.Users.AddRole;

public class AddRoleRequest : IRequest<AddRoleResponse>
{
    public int UserId { get; set; }
    public UserRole Role { get; set; }
}