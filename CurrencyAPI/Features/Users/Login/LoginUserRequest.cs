using MediatR;

namespace CurrencyAPI.Features.Users.Login;

public class LoginUserRequest : IRequest<LoginUserResponse>
{
    public string Email { get; set; }
    public string Password { get; set; }
}
