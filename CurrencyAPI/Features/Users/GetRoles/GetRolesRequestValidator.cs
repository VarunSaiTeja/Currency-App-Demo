using FluentValidation;

namespace CurrencyAPI.Features.Users.GetRoles;

public class GetRolesRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetRolesRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .EmailAddress()
            .WithMessage("EmailId is invalid");
    }
}
