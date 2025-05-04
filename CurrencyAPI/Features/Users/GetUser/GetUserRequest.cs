using MediatR;

namespace CurrencyAPI.Features.Users.GetUser;

public class GetUserRequest : IRequest<GetUserResponse>
{
    public string Email { get; set; }
}
