using CurrencyApp.Application.Features.Users.AddRole;

namespace CurrencyAPI.Tests.Features.Users.AddRole;
public class AddRoleRequestValidatorTest
{
    [Theory]
    [InlineData(1, 0)] // Valid: UserId > 0, Role in enum
    [InlineData(2, 2)] // Valid: UserId > 0, Role in enum
    public void ValidRequest_PassesValidation(int userId, int role)
    {
        var validator = new AddRoleRequestValidator();
        var request = new AddRoleRequest { UserId = userId, Role = (CurrencyApp.Data.Entities.UserRole)role };
        var result = validator.Validate(request);
        Assert.True(result.IsValid);
    }

    [Theory]
    [InlineData(0, 0, "UserId is required.")]
    [InlineData(-1, 0, "UserId is invalid")]
    [InlineData(1, 99, "Role is invalid.")]
    public void InvalidRequest_FailsValidation(int userId, int role, string expectedError)
    {
        var validator = new AddRoleRequestValidator();
        var request = new AddRoleRequest { UserId = userId, Role = (CurrencyApp.Data.Entities.UserRole)role };
        var result = validator.Validate(request);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.ErrorMessage == expectedError);
    }
}
