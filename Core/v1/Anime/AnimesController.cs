using Application.Commands;
using Asp.Versioning;
using AutoMapper;
using Core.Helpers.Constants;
using Core.v1.Anime.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.v1.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1_0)]
public class AnimesController(IMediator mediator, IMapper mapper) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost("anime")]
    public async Task<ActionResult<Guid?>> Create([FromBody] CreateAnimeRequest request)
    {
        var command = _mapper.Map<CreateAnimeCommand>(request);
        return await _mediator.Send(command);
    }
}