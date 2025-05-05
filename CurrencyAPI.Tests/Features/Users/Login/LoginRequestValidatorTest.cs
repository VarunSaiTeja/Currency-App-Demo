using CurrencyApp.Application.Features.Users.Login;

namespace CurrencyAPI.Tests.Features.Users.Login;
public class LoginRequestValidatorTest
{
    [Theory]
    [InlineData("user@example.com", "Password1!")]
    [InlineData("test@domain.com", "Abcdef1$")]
    public void ValidRequest_PassesValidation(string email, string password)
    {
        var validator = new LoginRequestValidator();
        var request = new LoginUserRequest { Email = email, Password = password };
        var result = validator.Validate(request);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "Password1!", "Email is required.")]
    [InlineData("invalidemail", "Password1!", "Invalid email format.")]
    [InlineData("user@example.com", "", "Password is required.")]
    [InlineData("user@example.com", "short", "Password must be at least 6 characters long.")]
    [InlineData("user@example.com", "password1!", "Password must contain at least one uppercase letter.")]
    [InlineData("user@example.com", "PASSWORD1!", "Password must contain at least one lowercase letter.")]
    [InlineData("user@example.com", "Password!", "Password must contain at least one number.")]
    [InlineData("user@example.com", "Password1", "Password must contain at least one special character.")]
    public void InvalidRequest_FailsValidation(string email, string password, string expectedError)
    {
        var validator = new LoginRequestValidator();
        var request = new LoginUserRequest { Email = email, Password = password };
        var result = validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedError);
    }
}
