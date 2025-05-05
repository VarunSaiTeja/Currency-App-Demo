using MediatR;

namespace CurrencyApp.Application.Features.Users.GetUser;

public class GetUserRequest : IRequest<GetUserResponse>
{
    public string Email { get; set; }
}
