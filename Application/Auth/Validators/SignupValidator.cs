using Application.Commands;
using Application.Shared.Base;
using FluentValidation;

namespace Application.Validators;

public class SignupValidator : ValidatorBase<SignupCommand>
{
    public SignupValidator()
    {
        RuleFor(command => command.Email)
            .EmailAddress();

        RuleFor(command => command.Password)
            .NotEmpty()
            .NotNull();

        RuleFor(command => command.PasswordConfirmation)
            .Equal(command => command.Password)
            .WithMessage("Passwords don't match");
    }
}