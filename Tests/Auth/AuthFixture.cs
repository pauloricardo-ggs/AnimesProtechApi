using Application.Commands;
using AutoFixture;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Tests.Auth.Fixtures;

public class AuthFixture
{
    private readonly Fixture fixture;

    public AuthFixture()
    {
        fixture = new Fixture();
    }

    public SigninCommand GetValidSigninCommand() => fixture.Create<SigninCommand>();
    public SignupCommand GetValidSignupCommand()
    {
        return new("test@test.com", "testPassword", "testPassword");
    }

    public SignupCommand GetInvalidSignupCommand() => fixture.Create<SignupCommand>();

    public Jwt GetValidJwt() => fixture.Create<Jwt>();
    public IdentityResult GetIdentityResultFailed() => IdentityResult.Failed(new IdentityError() 
    { 
        Code = "errorCode",
        Description = "errorDescription" 
    });
}