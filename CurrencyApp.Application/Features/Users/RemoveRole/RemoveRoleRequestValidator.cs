using FluentValidation;

namespace CurrencyApp.Application.Features.Users.RemoveRole;

public class RemoveRoleRequestValidator : AbstractValidator<RemoveRoleRequest>
{
    public RemoveRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .GreaterThan(0)
            .WithMessage("UserId is invalid");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role is invalid.");
    }
}
