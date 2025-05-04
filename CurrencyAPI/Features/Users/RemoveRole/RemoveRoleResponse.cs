using CurrencyApp.Data.Entities;

namespace CurrencyAPI.Features.Users.RemoveRole;

public class RemoveRoleResponse
{
    public int UserId { get; set; }
    public List<UserRole> Roles { get; set; }
}
