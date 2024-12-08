using Application.Commands;
using Application.Shared.Base;
using FluentValidation;

namespace Application.Validators;

public class UpdateAnimeValidator : ValidatorBase<UpdateAnimeCommand>
{
    public UpdateAnimeValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .NotNull();

        RuleFor(command => command.Name)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);

        RuleFor(command => command.Summary)
            .NotEmpty()
            .NotNull()
            .MaximumLength(500);

        RuleFor(command => command.Director)
            .NotEmpty()
            .NotNull()
            .MaximumLength(100);
    }
}