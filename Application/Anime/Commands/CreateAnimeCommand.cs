using Application.Shared;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class CreateAnimeCommand(string name, string summary, string director) : CommandBase, IRequest<ActionResult<Guid?>>
{
    public string Name { get; private set; } = name;
    public string Summary { get; private set; } = summary;
    public string Director { get; private set; } = director;

    public override void Validate()
    {
        var validationResult = new CreateAnimeValidator().Validate(this);
        AddErros(validationResult);
    }
}