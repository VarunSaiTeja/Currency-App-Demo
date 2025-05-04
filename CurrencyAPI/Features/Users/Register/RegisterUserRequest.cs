using MediatR;

namespace CurrencyAPI.Features.Users.Register;

public class RegisterUserRequest : IRequest<RegisterUserResponse>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
