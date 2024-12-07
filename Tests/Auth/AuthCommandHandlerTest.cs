using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using FluentAssertions;
using Domain.Interfaces;
using Tests.Extensions;
using Tests.Auth.Fixtures;
using Tests.Shared.Fixtures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Application.Handlers;
using MediatR;
using Application.Shared.Errors;
using Application;

namespace Tests.Auth;

public class AuthCommandHandlerTests
{
    private readonly AuthFixture _authFixture;
    private readonly UserFixture _userFixture;
    
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly Mock<IMediator> _mediatorMock;

    private readonly AuthCommandHandler _handler;

    public AuthCommandHandlerTests()
    {
        _authFixture = new AuthFixture();
        _userFixture = new UserFixture();

        _userManagerMock = new Mock<UserManager<User>>(
            Mock.Of<IUserStore<User>>(),
            Mock.Of<IOptions<IdentityOptions>>(), 
            Mock.Of<IPasswordHasher<User>>(),
            Array.Empty<IUserValidator<User>>(),
            Array.Empty<IPasswordValidator<User>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<ILogger<UserManager<User>>>());
        _jwtTokenGeneratorMock = new Mock<IJwtTokenGenerator>();
        _mediatorMock = new Mock<IMediator>();

        _handler = new AuthCommandHandler(_userManagerMock.Object, _jwtTokenGeneratorMock.Object, _mediatorMock.Object);
    }

    [Fact]
    public async Task Signin_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var command = _authFixture.GetValidSigninCommand();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Jwt>>();

        var resultObject = result.Result as UnauthorizedObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as string;
        resultObjectValue?.Should().Be(AuthError.InvalidCredentialsMessage);

        _jwtTokenGeneratorMock.ShouldNotHaveBeenCalled(generator => generator.Generate(It.IsAny<User>(), It.IsAny<UserManager<User>>()));
    }

    [Fact]
    public async Task Signin_InvalidEmailOrPassword_ReturnsUnauthorized()
    {
        // Arrange
        var command = _authFixture.GetValidSigninCommand();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(false);
        _userManagerMock.Setup(userManager => userManager.AccessFailedAsync(It.IsAny<User>()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Jwt>>();

        var resultObject = result.Result as UnauthorizedObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as string;
        resultObjectValue?.Should().Be(AuthError.InvalidCredentialsMessage);

        _userManagerMock.ShouldHaveBeenCalled(manager => manager.AccessFailedAsync(It.IsAny<User>()), Times.Once);
        _jwtTokenGeneratorMock.ShouldNotHaveBeenCalled(generator => generator.Generate(It.IsAny<User>(), It.IsAny<UserManager<User>>()));
    }

    [Fact]
    public async Task Signin_BlockedUser_ReturnsUnauthorized()
    {
        // Arrange
        var command = _authFixture.GetValidSigninCommand();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _userManagerMock.Setup(userManager => userManager.ResetAccessFailedCountAsync(It.IsAny<User>()));
        _userManagerMock.Setup(userManager => userManager.IsLockedOutAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Jwt>>();

        var resultObject = result.Result as ForbidObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as string;
        resultObjectValue?.Should().Be(AuthError.BlockedMessage);

        _userManagerMock.ShouldNotHaveBeenCalled(manager => manager.AccessFailedAsync(It.IsAny<User>()));
        _jwtTokenGeneratorMock.ShouldNotHaveBeenCalled(generator => generator.Generate(It.IsAny<User>(), It.IsAny<UserManager<User>>()));
    }

    [Fact]
    public async Task Signin_Success_ReturnsOk()
    {
        // Arrange
        var command = _authFixture.GetValidSigninCommand();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);
        _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(true);
        _userManagerMock.Setup(userManager => userManager.ResetAccessFailedCountAsync(It.IsAny<User>()));
        _userManagerMock.Setup(userManager => userManager.IsLockedOutAsync(It.IsAny<User>()))
            .ReturnsAsync(false);
        _jwtTokenGeneratorMock.Setup(generator => generator.Generate(user, _userManagerMock.Object))
            .ReturnsAsync(_authFixture.GetValidJwt());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Jwt>>();

        var resultObject = result.Result as OkObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as Jwt;
        resultObjectValue?.Token.Should().NotBeNullOrEmpty().And.BeOfType<string>();
        resultObjectValue?.ExpiresIn.Should().NotBeNullOrEmpty().And.BeOfType<string>();

        _userManagerMock.ShouldNotHaveBeenCalled(manager => manager.AccessFailedAsync(It.IsAny<User>()));
        _userManagerMock.ShouldHaveBeenCalled(manager => manager.ResetAccessFailedCountAsync(It.IsAny<User>()), Times.Once);
        _jwtTokenGeneratorMock.ShouldHaveBeenCalled(generator => generator.Generate(It.IsAny<User>(), It.IsAny<UserManager<User>>()), Times.Once);
    }

    [Fact]
    public async Task Signup_InvalidCommand_ReturnsBadRequest()
    {
        // Arrange
        var command = _authFixture.GetInvalidSignupCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Guid>>();

        var resultObject = result.Result as BadRequestObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as string;
        resultObjectValue?.Split("\n").Should().HaveCount(2);

        _userManagerMock.ShouldNotHaveBeenCalled(manager => manager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task Signup_ErrorWhileCreatingUser_ReturnsBadRequest()
    {
        // Arrange
        var command = _authFixture.GetInvalidSignupCommand();
        var identityResult = _authFixture.GetIdentityResultFailed();
        _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Guid>>();

        var resultObject = result.Result as BadRequestObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as IEnumerable<IdentityError>;
        resultObject.Should().NotBeNull();

        resultObjectValue?.FirstOrDefault()?.Code.Should().Be(identityResult.Errors.FirstOrDefault()?.Code);
    }

    [Fact]
    public async Task Signup_Success_ReturnsCreatedWithJwt()
    {
        // Arrange
        var command = _authFixture.GetValidSignupCommand();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().BeOfType<ActionResult<Guid>>();

        var resultObject = result.Result as CreatedObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as Guid?;
        resultObject.Should().NotBeNull();

        _userManagerMock.ShouldHaveBeenCalled(manager => manager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
    }
}