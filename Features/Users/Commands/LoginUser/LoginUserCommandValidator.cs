using FluentValidation;

namespace ManagementSystem.Api.Features.Users.Commands.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email is invalid")
            .MaximumLength(50).WithMessage("Maximum length 50 characters");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}