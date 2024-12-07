using Application.Commands;
using Application.Shared;
using Application.Shared.Errors;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Application.Handlers;

public class AuthCommandHandler(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator, IMediator mediator) : CommandHandlerBase(mediator),
    IRequestHandler<SignupCommand, ActionResult<Guid>>,
    IRequestHandler<SigninCommand, ActionResult<Jwt>>
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<ActionResult<Guid>> Handle(SignupCommand request, CancellationToken cancellationToken)
    {
        request.Validate();
        if (!request.Valid)
        {
            return BaseError.InvalidArguments(request.Errors).Result;
        }

        var user = new User(request.Email);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BaseError.InvalidArguments(result.Errors).Result;
        }

        return new CreatedObjectResult(user.Id);
    }

    public async Task<ActionResult<Jwt>> Handle(SigninCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            return AuthError.InvalidCredentials.Result;
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            await _userManager.AccessFailedAsync(user);
            return AuthError.InvalidCredentials.Result;
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            return AuthError.Blocked.Result;
        }

        await _userManager.ResetAccessFailedCountAsync(user);
        return new OkObjectResult(await _jwtTokenGenerator.Generate(user, _userManager));
    }
}