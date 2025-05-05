using CurrencyApp.Application.Features.Users.RefreshAccessToken;

namespace CurrencyAPI.Tests.Features.Users.RefreshUserToken;
public class RefreshTokenRequestValidatorTest
{
    [Theory]
    [InlineData("sometoken")]
    [InlineData("1234567890abcdef")]
    public void ValidRequest_PassesValidation(string refreshToken)
    {
        var validator = new RefreshTokenRequestValidator();
        var request = new RefreshTokenRequest { RefreshToken = refreshToken };
        var result = validator.Validate(request);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void InvalidRequest_FailsValidation(string refreshToken)
    {
        var validator = new RefreshTokenRequestValidator();
        var request = new RefreshTokenRequest { RefreshToken = refreshToken };
        var result = validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == "Refresh token is required.");
    }
}
