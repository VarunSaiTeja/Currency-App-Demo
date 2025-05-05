using CurrencyApp.Application.Features.Users.Register;

namespace CurrencyAPI.Tests.Features.Users.Register;
public class RegisterUserValidatorTest
{
    [Theory]
    [InlineData("John Doe", "user@example.com", "Password1!")]
    [InlineData("Jane", "test@domain.com", "Abcdef1$")]
    public void ValidRequest_PassesValidation(string name, string email, string password)
    {
        var validator = new RegisterUserValidator();
        var request = new RegisterUserRequest { Name = name, Email = email, Password = password };
        var result = validator.Validate(request);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "user@example.com", "Password1!", "Name is required.")]
    [InlineData("Jo", "user@example.com", "Password1!", "Name must be at least 3 characters long.")]
    [InlineData("John Doe", "", "Password1!", "Email is required.")]
    [InlineData("John Doe", "invalidemail", "Password1!", "Invalid email format.")]
    [InlineData("John Doe", "user@example.com", "", "Password is required.")]
    [InlineData("John Doe", "user@example.com", "short", "Password must be at least 6 characters long.")]
    [InlineData("John Doe", "user@example.com", "password1!", "Password must contain at least one uppercase letter.")]
    [InlineData("John Doe", "user@example.com", "PASSWORD1!", "Password must contain at least one lowercase letter.")]
    [InlineData("John Doe", "user@example.com", "Password!", "Password must contain at least one number.")]
    [InlineData("John Doe", "user@example.com", "Password1", "Password must contain at least one special character.")]
    public void InvalidRequest_FailsValidation(string name, string email, string password, string expectedError)
    {
        var validator = new RegisterUserValidator();
        var request = new RegisterUserRequest { Name = name, Email = email, Password = password };
        var result = validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedError);
    }
}
