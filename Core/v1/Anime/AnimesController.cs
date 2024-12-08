using Application.Commands;
using Application.Queries;
using Asp.Versioning;
using AutoMapper;
using Core.Helpers;
using Core.Helpers.Attributes;
using Core.Helpers.Constants;
using Core.v1.Anime.Dtos;
using Domain.Constants;
using Domain.Helpers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [RolesAuthorize(RoleConstant.ADMIN)]
    public async Task<ActionResult<Guid?>> Create([FromBody] AnimeDto request)
    {
        var command = _mapper.Map<CreateAnimeCommand>(request);
        return await _mediator.Send(command);
    }

    [HttpGet("animes")]
    [Authorize]
    [Filterable]
    public async Task<ActionResult<PagedList<AnimeDetailsDto>>> List()
    {
        var (page, pageSize) = HttpContext.GetQueryPagination();
        var filters = HttpContext.GetQueryFilters();
        var animes = await _animeQueries.List(filters, page, pageSize);
        return Ok(_mapper.Map<PagedList<AnimeDetailsDto>>(animes));
    }

    [HttpPut("anime/{animeId}")]
    [RolesAuthorize(RoleConstant.ADMIN)]
    public async Task<ActionResult> Update([FromRoute] Guid animeId, [FromBody] AnimeDto request)
    {
        var command = _mapper.Map<UpdateAnimeCommand>(request);
        command.AddId(animeId);
        return await _mediator.Send(command);
    }

    [HttpDelete("anime/{animeId}")]
    [RolesAuthorize(RoleConstant.ADMIN)]
    public async Task<ActionResult<Guid?>> Delete([FromRoute] Guid animeId)
    {
        var command = new DeleteAnimeCommand(animeId);
        return await _mediator.Send(command);
    }
}