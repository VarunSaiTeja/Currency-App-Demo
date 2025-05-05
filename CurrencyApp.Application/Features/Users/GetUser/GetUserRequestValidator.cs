using FluentValidation;

namespace CurrencyApp.Application.Features.Users.GetUser;

public class GetUserRequestValidator : AbstractValidator<GetUserRequest>
{
    public GetUserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .EmailAddress()
            .WithMessage("EmailId is invalid");
    }
}
