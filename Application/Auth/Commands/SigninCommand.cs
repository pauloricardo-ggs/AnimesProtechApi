using Application.Shared;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class SigninCommand(string email, string password) : CommandBase, IRequest<ActionResult<Jwt>>
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
}