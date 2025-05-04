using MediatR;

namespace CurrencyAPI.Features.Users.GetRoles;

public class GetUserRequest : IRequest<GetUserResponse>
{
    public string Email { get; set; }
}
