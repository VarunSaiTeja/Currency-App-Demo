using MediatR;

namespace CurrencyAPI.Features.Users.RefreshAccessToken;

public class RefreshTokenRequest : IRequest<RefreshTokenResponse>
{
    public string RefreshToken { get; set; }
}