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
using Domain.Interfaces;

namespace Tests.Anime;

public class CreateAnimeCommandHandlerTest
{
    private readonly AnimeFixture _animeFixture;
    
    private readonly Mock<IRequestLogger> _loggerMock;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IAnimeQueries> _animeQueriesMock;
    private readonly Mock<IAnimeRepository> _animeRepositoryMock;

    private readonly AnimeCommandHandler _handler;

    public CreateAnimeCommandHandlerTest()
    {
        _animeFixture = new AnimeFixture();

        _loggerMock = new Mock<IRequestLogger>();
        _mediatorMock = new Mock<IMediator>();
        _animeQueriesMock = new Mock<IAnimeQueries>();
        _animeRepositoryMock = new Mock<IAnimeRepository>();

        _handler = new AnimeCommandHandler(_loggerMock.Object, _mediatorMock.Object, _animeQueriesMock.Object, _animeRepositoryMock.Object);
    }

    [Fact]
    public async Task Create_InvalidCommand_ReturnsBadRequest()
    {
        // Arrange
        var command = _animeFixture.GetInvalidCreateCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Guid?>>();

        var resultObject = result.Result as BadRequestObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as string;
        resultObjectValue?.Split("\n").Should().HaveCount(3);

        _animeQueriesMock.ShouldNotHaveBeenCalled(manager => manager.GetByName(It.IsAny<string>()));
        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Create(It.IsAny<Domain.Entities.Anime>()));
    }

    [Fact]
    public async Task Create_NameAlreadyInUse_ReturnsConflict()
    {
        // Arrange
        var command = _animeFixture.GetValidCreateCommand();
        var anime = _animeFixture.GetValidAnime();
        _animeQueriesMock.Setup(queries => queries.GetByName(It.IsAny<string>()))
            .ReturnsAsync(anime);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Guid?>>();

        var resultObject = result.Result as ConflictObjectResult;
        resultObject.Should().NotBeNull();

        _animeRepositoryMock.ShouldNotHaveBeenCalled(manager => manager.Create(It.IsAny<Domain.Entities.Anime>()));
    }

    [Fact]
    public async Task Create_ErrorWhileSaving_ReturnsUnexpectedBehavior()
    {
        // Arrange
        var command = _animeFixture.GetValidCreateCommand();
        _animeQueriesMock.Setup(queries => queries.GetByName(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Anime?)null);
        _animeRepositoryMock.Setup(repository => repository.Create(It.IsAny<Domain.Entities.Anime>()));
        _animeRepositoryMock.Setup(repository => repository.Save())
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Guid?>>();

        var resultObject = result.Result as InternalServerErrorObjectResult;
        resultObject.Should().NotBeNull();

        _animeRepositoryMock.ShouldHaveBeenCalled(manager => manager.Create(It.IsAny<Domain.Entities.Anime>()), Times.Once);
    }

    [Fact]
    public async Task Create_Success_ReturnsCreatedWithAnimeId()
    {
        // Arrange
        var command = _animeFixture.GetValidCreateCommand();
        _animeQueriesMock.Setup(queries => queries.GetByName(It.IsAny<string>()))
            .ReturnsAsync((Domain.Entities.Anime?)null);
        _animeRepositoryMock.Setup(repository => repository.Create(It.IsAny<Domain.Entities.Anime>()));
        _animeRepositoryMock.Setup(repository => repository.Save())
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Guid?>>();

        var resultObject = result.Result as CreatedObjectResult;
        resultObject.Should().NotBeNull();

        _animeRepositoryMock.ShouldHaveBeenCalled(manager => manager.Create(It.IsAny<Domain.Entities.Anime>()), Times.Once);
    }
}