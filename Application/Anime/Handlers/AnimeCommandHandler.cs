using Application.Commands;
using Application.Errors;
using Application.Queries;
using Application.Shared;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers;

public class AnimeCommandHandler(
    IMediator mediator,
    IAnimeQueries animeQueries,
    IAnimeRepository animeRepository) 
    : CommandHandlerBase(mediator),
    IRequestHandler<CreateAnimeCommand, ActionResult<Guid?>>,
    IRequestHandler<UpdateAnimeCommand, ActionResult>,
    IRequestHandler<DeleteAnimeCommand, ActionResult>
{
    private readonly IAnimeQueries _animeQueries = animeQueries;
    private readonly IAnimeRepository _animeRepository = animeRepository;

    public async Task<ActionResult<Guid?>> Handle(CreateAnimeCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        if (!request.Valid)
        {
            return BaseError.InvalidArguments(request.Errors).Result;
        }

        var anime = await _animeQueries.GetByName(request.Name);
        if (anime != null)
        {
            return AnimeError.NameAlreadyExists.Result;
        }
        
        anime = new Anime(request.Name, request.Summary, request.Director);

        _animeRepository.Create(anime);
        
        if (!await _animeRepository.Save())
        {
            return BaseError.UnexpectedBehavior.Result;
        }

        return new CreatedObjectResult(anime.Id);
    }

    public async Task<ActionResult> Handle(UpdateAnimeCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        if (!request.Valid)
        {
            return BaseError.InvalidArguments(request.Errors).Result;
        }

        var anime = await _animeQueries.GetById(request.Id!.Value);
        if (anime == null)
        {
            return BaseError.ResourceNotFound.Result;
        }

        var animeWithThisName = await _animeQueries.GetByName(request.Name);
        if (animeWithThisName != null && animeWithThisName.Id != request.Id)
        {
            return AnimeError.NameAlreadyExists.Result;
        }
        
        anime.Update(request.Name, request.Summary, request.Director);

        _animeRepository.Update(anime);
        
        if (!await _animeRepository.Save())
        {
            return BaseError.UnexpectedBehavior.Result;
        }

        return new NoContentResult();
    }

    public async Task<ActionResult> Handle(DeleteAnimeCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        if (!request.Valid)
        {
            return BaseError.InvalidArguments(request.Errors).Result;
        }

        var anime = await _animeQueries.GetById(request.Id!.Value);
        if (anime == null)
        {
            return BaseError.ResourceNotFound.Result;
        }

        _animeRepository.Delete(anime);
        
        if (!await _animeRepository.Save())
        {
            return BaseError.UnexpectedBehavior.Result;
        }

        return new NoContentResult();
    }
}