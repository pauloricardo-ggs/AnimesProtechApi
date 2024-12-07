using Core.Dtos;
using Core.Dtos.Requests;
using Core.Helpers;
using Core.Helpers.Erros;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Core.v1.Controllers;

[ApiController]
public class AuthController(UserManager<User> userManager, IJwtTokenGenerator jwtTokenGenerator) : ControllerBase
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    [HttpPost("auth/signup")]
    public async Task<ActionResult<IdDto>> Signup([FromBody] SignupRequest request)
    {
        if (request.Password != request.PasswordConfirmation)
        {
            return BadRequest(AuthError.PasswordsDontMatch);
        }

        var user = new User(request.Email);
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        return Created("", new IdDto { Id = user.Id.ToString() });
    }

    [HttpPost("auth/signin")]
    public async Task<ActionResult<Jwt>> Signin([FromBody] SigninRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            return Unauthorized(AuthError.InvalidCredentials);
        }

        if (!await _userManager.CheckPasswordAsync(user, request.Password))
        {
            await _userManager.AccessFailedAsync(user);
            return Unauthorized(AuthError.InvalidCredentials);
        }

        await _userManager.ResetAccessFailedCountAsync(user);

        if (await _userManager.IsLockedOutAsync(user))
        {
            return Unauthorized(AuthError.Blocked);
        }

        return Ok(await _jwtTokenGenerator.Generate(user, _userManager));
    }
}