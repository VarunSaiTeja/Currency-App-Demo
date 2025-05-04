namespace CurrencyAPI.Features.Users.Login;

public class LoginUserResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public DateTime AccessTokenExpiresOn { get; set; }
    public DateTime RefreshTokenExpiresOn { get; set; }
}