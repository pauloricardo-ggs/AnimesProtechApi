using Application.Shared;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class UpdateAnimeCommand : CommandBase, IRequest<ActionResult>
{
    public UpdateAnimeCommand()
    {
        Id = Guid.Empty;
        Name = string.Empty;
        Summary = string.Empty;
        Director = string.Empty;
    }
    
    public UpdateAnimeCommand(Guid id, string name, string summary, string director)
    {
        Id = id;
        Name = name;
        Summary = summary;
        Director = director;
    }

    public Guid? Id { get; private set; }
    public string Name { get; private set; }
    public string Summary { get; private set; }
    public string Director { get; private set; }

    public void AddId(Guid id)
        => Id = id;

    public override void Validate()
    {
        var validationResult = new UpdateAnimeValidator().Validate(this);
        AddErros(validationResult);
    }
}