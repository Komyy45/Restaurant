using FluentValidation;

namespace Restaurant.Application.Features.Authentication.Commands.Register;

internal sealed class RegisterCommandValidator : AbstractValidator<Features.Authentication.Commands.Register.RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(e => e.Email)
            .EmailAddress()
            .WithMessage("Please enter a valid email address.");

        RuleFor(e => e.Password)
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[\W_]").WithMessage("Password must contain at least one symbol.");
        
        RuleFor(e => e.UserName)
            .MinimumLength(3)
            .WithMessage("Username must be at least 3 characters long.")
            .MaximumLength(20)
            .WithMessage("Username must not exceed 20 characters.");
    }
}