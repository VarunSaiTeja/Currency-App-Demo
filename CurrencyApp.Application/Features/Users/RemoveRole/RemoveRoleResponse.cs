using CurrencyApp.Data.Entities;

namespace CurrencyApp.Application.Features.Users.RemoveRole;

public class RemoveRoleResponse
{
    public int UserId { get; set; }
    public List<UserRole> Roles { get; set; }
}
