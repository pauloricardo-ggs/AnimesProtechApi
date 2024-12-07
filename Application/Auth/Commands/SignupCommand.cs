using Application.Shared;
using Application.Validators;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Application.Commands;

public class SignupCommand(string email, string password, string passwordConfirmation) : CommandBase, IRequest<ActionResult<Guid>>
{
    public string Email { get; private set; } = email;
    public string Password { get; private set; } = password;
    public string PasswordConfirmation { get; private set; } = passwordConfirmation;

    public override void Validate()
    {
        var validationResult = new SignupValidator().Validate(this);
        AddErros(validationResult);
    }
}