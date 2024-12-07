using AutoFixture;
using Domain.Entities;

namespace Tests.Shared.Fixtures;

public class UserFixture
{
    private readonly Fixture fixture;

    public UserFixture()
    {
        fixture = new Fixture();
    }

    public User GetValidUser() => fixture.Create<User>();
}