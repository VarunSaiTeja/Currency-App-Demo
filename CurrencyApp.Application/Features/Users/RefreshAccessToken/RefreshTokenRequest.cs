using MediatR;

namespace CurrencyApp.Application.Features.Users.RefreshAccessToken;

public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
{
    public string RefreshToken { get; set; }
}