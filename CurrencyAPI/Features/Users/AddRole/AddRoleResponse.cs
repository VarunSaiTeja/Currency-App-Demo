using CurrencyApp.Data.Entities;

namespace CurrencyAPI.Features.Users.AddRole;

public class AddRoleResponse
{
    public int UserId { get; set; }
    public List<UserRole> Roles { get; set; }
}
