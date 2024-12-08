using Application.Commands;
using Application.Queries;
using Asp.Versioning;
using AutoMapper;
using Core.Helpers.Constants;
using Core.v1.Anime.Dtos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.v1.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1_0)]
public class AnimesController(IMediator mediator, IMapper mapper, IAnimeQueries animeQueries) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;
    private readonly IAnimeQueries _animeQueries = animeQueries;

    [HttpPost("anime")]
    public async Task<ActionResult<Guid?>> Create([FromBody] AnimeDto request)
    {
        var command = _mapper.Map<CreateAnimeCommand>(request);
        return await _mediator.Send(command);
    }

    [HttpGet("animes")]
    public async Task<ActionResult<AnimesDetailsDto>> List()
    {
        var animes = await _animeQueries.List();
        return Ok(_mapper.Map<AnimesDetailsDto>(animes));
    }

    [HttpPut("anime/{animeId}")]
    public async Task<ActionResult> Update([FromRoute] Guid animeId, [FromBody] AnimeDto request)
    {
        var command = _mapper.Map<UpdateAnimeCommand>(request);
        command.AddId(animeId);
        return await _mediator.Send(command);
    }

    [HttpDelete("anime/{animeId}")]
    public async Task<ActionResult<Guid?>> Delete([FromRoute] Guid animeId)
    {
        var command = new DeleteAnimeCommand(animeId);
        return await _mediator.Send(command);
    }
}