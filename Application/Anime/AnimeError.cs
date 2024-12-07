using Application.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Application.Errors;

public class AnimeError : BaseError
{
    private AnimeError(ObjectResult result) : base(result) { }

    public static AnimeError NameAlreadyExists = new (new ConflictObjectResult("Already exists an anime with this name"));
}