using Application.Commands;
using Application.Shared.Base;
using FluentValidation;

namespace Application.Validators;

public class DeleteAnimeValidator : ValidatorBase<DeleteAnimeCommand>
{
    public DeleteAnimeValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .NotNull();
    }
}