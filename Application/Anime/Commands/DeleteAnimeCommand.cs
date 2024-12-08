using Application.Shared;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class DeleteAnimeCommand(Guid? id) : CommandBase, IRequest<ActionResult>
{
    public Guid? Id { get; private set; } = id;

    public override void Validate()
    {
        var validationResult = new DeleteAnimeValidator().Validate(this);
        AddErros(validationResult);
    }
}