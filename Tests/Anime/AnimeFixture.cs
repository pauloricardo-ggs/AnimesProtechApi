using Application.Commands;
using AutoFixture;

namespace Tests.Anime.Fixtures;

public class AnimeFixture
{
    private readonly Fixture fixture;

    public AnimeFixture()
    {
        fixture = new Fixture();
    }

    public CreateAnimeCommand GetValidCreateCommand() => fixture.Create<CreateAnimeCommand>();
    public CreateAnimeCommand GetInvalidCreateCommand() => new("", "", "");

    public Domain.Entities.Anime GetValidAnime() => fixture.Create<Domain.Entities.Anime>();
}