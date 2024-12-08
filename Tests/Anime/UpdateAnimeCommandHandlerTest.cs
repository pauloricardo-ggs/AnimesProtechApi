using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Tests.Extensions;
using Application.Handlers;
using MediatR;
using Application.Queries;
using Domain.Interfaces.Repositories;
using Tests.Anime.Fixtures;
using Application;

namespace Tests.Anime;

public class UpdateAnimeCommandHandlerTest
{
    private readonly AnimeFixture _animeFixture;
    
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IAnimeQueries> _animeQueriesMock;
    private readonly Mock<IAnimeRepository> _animeRepositoryMock;

    private readonly AnimeCommandHandler _handler;

    public UpdateAnimeCommandHandlerTest()
    {
        _animeFixture = new AnimeFixture();

        _mediatorMock = new Mock<IMediator>();
        _animeQueriesMock = new Mock<IAnimeQueries>();
        _animeRepositoryMock = new Mock<IAnimeRepository>();

        _handler = new AnimeCommandHandler(_mediatorMock.Object, _animeQueriesMock.Object, _animeRepositoryMock.Object);
    }

    [Fact]
    public async Task Update_InvalidCommand_ReturnsBadRequest()
    {
        // Arrange
        var command = _animeFixture.GetInvalidUpdateCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        var resultObject = result as BadRequestObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as string;
        resultObjectValue?.Split("\n").Should().HaveCount(3);

        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Update(It.IsAny<Domain.Entities.Anime>()));
        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Save());
    }

    [Fact]
    public async Task Update_AnimeNotFound_ReturnsNotFound()
    {
        // Arrange
        var command = _animeFixture.GetValidUpdateCommand();
        _animeQueriesMock.Setup(queries => queries.GetById(It.IsAny<Guid>()))
            .ReturnsAsync((Domain.Entities.Anime?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NotFoundObjectResult>();

        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Update(It.IsAny<Domain.Entities.Anime>()));
        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Save());
    }

    [Fact]
    public async Task Update_NameAlreadyInUse_ReturnsConflict()
    {
        // Arrange
        var command = _animeFixture.GetValidUpdateCommand();
        var anime = _animeFixture.GetValidAnime();
        _animeQueriesMock.Setup(queries => queries.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(anime);
        _animeQueriesMock.Setup(queries => queries.GetByName(It.IsAny<string>()))
            .ReturnsAsync(anime);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ConflictObjectResult>();

        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Update(It.IsAny<Domain.Entities.Anime>()));
        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Save());
    }

    [Fact]
    public async Task Update_ErrorWhileSaving_ReturnsUnexpectedBehavior()
    {
        // Arrange
        var command = _animeFixture.GetValidUpdateCommand();
        var anime = _animeFixture.GetValidAnime();
        _animeQueriesMock.Setup(queries => queries.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(anime);
        _animeQueriesMock.Setup(queries => queries.GetByName(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Anime?)null);
        _animeRepositoryMock.Setup(repository => repository.Update(It.IsAny<Domain.Entities.Anime>()));
        _animeRepositoryMock.Setup(repository => repository.Save())
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<InternalServerErrorObjectResult>();

        _animeRepositoryMock.ShouldHaveBeenCalled(manager => manager.Update(It.IsAny<Domain.Entities.Anime>()), Times.Once);
        _animeRepositoryMock.ShouldHaveBeenCalled(manager => manager.Save(), Times.Once);
    }

    [Fact]
    public async Task Update_Success_ReturnsNoContent()
    {
        // Arrange
        var command = _animeFixture.GetValidUpdateCommand();
        var anime = _animeFixture.GetValidAnime();
        _animeQueriesMock.Setup(queries => queries.GetById(It.IsAny<Guid>()))
            .ReturnsAsync(anime);
        _animeQueriesMock.Setup(queries => queries.GetByName(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Anime?)null);
        _animeRepositoryMock.Setup(repository => repository.Update(It.IsAny<Domain.Entities.Anime>()));
        _animeRepositoryMock.Setup(repository => repository.Save())
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<NoContentResult>();

        _animeRepositoryMock.ShouldHaveBeenCalled(manager => manager.Update(It.IsAny<Domain.Entities.Anime>()), Times.Once);
        _animeRepositoryMock.ShouldHaveBeenCalled(manager => manager.Save(), Times.Once);
    }
}