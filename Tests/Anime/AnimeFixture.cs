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
    
    public UpdateAnimeCommand GetValidUpdateCommand() 
        => new(Guid.NewGuid(), "Anime Name", "Anime Summary", "Anime Director");
    public UpdateAnimeCommand GetInvalidUpdateCommand() 
        => new(Guid.Empty, string.Empty, string.Empty, string.Empty);

    public DeleteAnimeCommand GetValidDeleteCommand() => new(Guid.NewGuid());
    public DeleteAnimeCommand GetInvalidDeleteCommand() => new(null);

    public Domain.Entities.Anime GetValidAnime() => fixture.Create<Domain.Entities.Anime>();
}