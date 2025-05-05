namespace CurrencyApp.Application.Features.Users.RefreshAccessToken;

public class RefreshTokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresOn { get; set; }
    public DateTime RefreshTokenExpiresOn { get; set; }
}
