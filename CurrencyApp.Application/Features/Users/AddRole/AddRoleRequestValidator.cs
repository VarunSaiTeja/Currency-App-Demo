using FluentValidation;

namespace CurrencyApp.Application.Features.Users.AddRole;

public class AddRoleRequestValidator : AbstractValidator<AddRoleRequest>
{
    public AddRoleRequestValidator()
    {
        RuleFor(x => x.UserId)
            .Cascade(CascadeMode.StopOnFirstFailure)
            .NotEmpty()
            .WithMessage("UserId is required.")
            .GreaterThan(0)
            .WithMessage("UserId is invalid");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role is invalid.");
    }
}
