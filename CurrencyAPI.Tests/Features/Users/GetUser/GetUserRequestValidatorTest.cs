using CurrencyApp.Application.Features.Users.GetUser;

namespace CurrencyAPI.Tests.Features.Users.GetUser;
public class GetUserRequestValidatorTest
{
    [Theory]
    [InlineData("user@example.com")]
    [InlineData("test@domain.com")]
    public void ValidRequest_PassesValidation(string email)
    {
        var validator = new GetUserRequestValidator();
        var request = new GetUserRequest { Email = email };
        var result = validator.Validate(request);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData("", "UserId is required.")]
    [InlineData("invalidemail", "EmailId is invalid")]
    public void InvalidRequest_FailsValidation(string email, string expectedError)
    {
        var validator = new GetUserRequestValidator();
        var request = new GetUserRequest { Email = email };
        var result = validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedError);
    }
}
