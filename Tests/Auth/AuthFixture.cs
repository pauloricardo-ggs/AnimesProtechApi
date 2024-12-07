using AutoFixture;
using Core.Dtos.Requests;
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

    public SigninRequest GetValidSigninRequest() => fixture.Create<SigninRequest>();
    public SignupRequest GetValidSignupRequest() => new() 
    {
        Email = "test@test.com",
        Password = "123456",
        PasswordConfirmation = "123456"
    };
    public SignupRequest GetInvalidSignupRequest_PasswordsDontMatch() => fixture.Create<SignupRequest>();
    public Jwt GetValidJwt() => fixture.Create<Jwt>();
    public IdentityResult GetIdentityResultFailed() => IdentityResult.Failed(new IdentityError() 
    { 
        Code = "errorCode",
        Description = "errorDescription" 
    });
}