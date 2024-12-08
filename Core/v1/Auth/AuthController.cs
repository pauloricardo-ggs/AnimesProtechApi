using Application.Commands;
using Asp.Versioning;
using AutoMapper;
using Core.Helpers.Constants;
using Core.v1.Auth.Dtos;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Core.v1.Controllers;

[ApiController]
[ApiVersion(ApiVersions.V1_0)]
public class AuthController(IMediator mediator, IMapper mapper) : ControllerBase
{   
    private readonly IMediator _mediator = mediator;
    private readonly IMapper _mapper = mapper;

    [HttpPost("auth/signup")]
    public async Task<ActionResult<Guid>> Signup([FromBody] SignupDto request)
    {
        var command = _mapper.Map<SignupCommand>(request);
        return await _mediator.Send(command);
    }

    [HttpPost("auth/signin")]
    public async Task<ActionResult<Jwt>> Signin([FromBody] SigninDto request)
    {
        var command = _mapper.Map<SigninCommand>(request);
        return await _mediator.Send(command);
    }
}