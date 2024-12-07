using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Core.v1.Controllers;
using Microsoft.AspNetCore.Identity;
using FluentAssertions;
using Core.Helpers.Erros;
using Domain.Interfaces;
using Tests.Extensions;
using Tests.Auth.Fixtures;
using Tests.Shared.Fixtures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Core.Dtos;

namespace Tests.Auth;

public class AuthControllerTests
{
    private readonly AuthFixture _authFixture;
    private readonly UserFixture _userFixture;
    private readonly Mock<UserManager<User>> _userManagerMock;
    private readonly Mock<IJwtTokenGenerator> _jwtTokenGeneratorMock;
    private readonly AuthController _controller;

    public AuthControllerTests()
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

        _controller = new AuthController(_userManagerMock.Object, _jwtTokenGeneratorMock.Object);
    }

    [Fact]
    public async Task Signin_UserNotFound_ReturnsUnauthorized()
    {
        // Arrange
        var request = _authFixture.GetValidSigninRequest();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(request.Email))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _controller.Signin(request);

        // Assert
        result.Should().BeOfType<ActionResult<Jwt>>();

        var resultObject = result.Result as UnauthorizedObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as AuthError;
        resultObjectValue?.Code.Should().Be(AuthError.InvalidCredentials.Code);
        resultObjectValue?.Message.Should().Be(AuthError.InvalidCredentials.Message);

        _jwtTokenGeneratorMock.ShouldNotHaveBeenCalled(generator => generator.Generate(It.IsAny<User>(), It.IsAny<UserManager<User>>()));
    }

    [Fact]
    public async Task Signin_InvalidEmailOrPassword_ReturnsUnauthorized()
    {
        // Arrange
        var request = _authFixture.GetValidSigninRequest();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);
        _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(false);
        _userManagerMock.Setup(userManager => userManager.AccessFailedAsync(user));

        // Act
        var result = await _controller.Signin(request);

        // Assert
        result.Should().BeOfType<ActionResult<Jwt>>();

        var resultObject = result.Result as UnauthorizedObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as AuthError;
        resultObjectValue?.Code.Should().Be(AuthError.InvalidCredentials.Code);
        resultObjectValue?.Message.Should().Be(AuthError.InvalidCredentials.Message);

        _userManagerMock.ShouldHaveBeenCalled(manager => manager.AccessFailedAsync(It.IsAny<User>()), Times.Once);
        _jwtTokenGeneratorMock.ShouldNotHaveBeenCalled(generator => generator.Generate(It.IsAny<User>(), It.IsAny<UserManager<User>>()));
    }

    [Fact]
    public async Task Signin_BlockedUser_ReturnsUnauthorized()
    {
        // Arrange
        var request = _authFixture.GetValidSigninRequest();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);
        _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(true);
        _userManagerMock.Setup(userManager => userManager.ResetAccessFailedCountAsync(user));
        _userManagerMock.Setup(userManager => userManager.IsLockedOutAsync(user))
            .ReturnsAsync(true);

        // Act
        var result = await _controller.Signin(request);

        // Assert
        result.Should().BeOfType<ActionResult<Jwt>>();

        var resultObject = result.Result as UnauthorizedObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as AuthError;
        resultObjectValue?.Code.Should().Be(AuthError.Blocked.Code);
        resultObjectValue?.Message.Should().Be(AuthError.Blocked.Message);

        _userManagerMock.ShouldNotHaveBeenCalled(manager => manager.AccessFailedAsync(It.IsAny<User>()));
        _userManagerMock.ShouldHaveBeenCalled(manager => manager.ResetAccessFailedCountAsync(It.IsAny<User>()), Times.Once);
        _jwtTokenGeneratorMock.ShouldNotHaveBeenCalled(generator => generator.Generate(It.IsAny<User>(), It.IsAny<UserManager<User>>()));
    }

    [Fact]
    public async Task Signin_Success_ReturnsOk()
    {
        // Arrange
        var request = _authFixture.GetValidSigninRequest();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.FindByEmailAsync(request.Email))
            .ReturnsAsync(user);
        _userManagerMock.Setup(userManager => userManager.CheckPasswordAsync(user, request.Password))
            .ReturnsAsync(true);
        _userManagerMock.Setup(userManager => userManager.ResetAccessFailedCountAsync(user));
        _userManagerMock.Setup(userManager => userManager.IsLockedOutAsync(user))
            .ReturnsAsync(false);
        _jwtTokenGeneratorMock.Setup(generator => generator.Generate(user, _userManagerMock.Object))
            .Returns(Task.FromResult(_authFixture.GetValidJwt()));

        // Act
        var result = await _controller.Signin(request);

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
    public async Task Signup_PasswordsDontMatch_ReturnsBadRequest()
    {
        // Arrange
        var request = _authFixture.GetInvalidSignupRequest_PasswordsDontMatch();

        // Act
        var result = await _controller.Signup(request);

        // Assert
        result.Should().BeOfType<ActionResult<IdDto>>();

        var resultObject = result.Result as BadRequestObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as AuthError;
        resultObjectValue?.Code.Should().Be(AuthError.PasswordsDontMatch.Code);
        resultObjectValue?.Message.Should().Be(AuthError.PasswordsDontMatch.Message);

        _userManagerMock.ShouldNotHaveBeenCalled(manager => manager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()));
    }

    [Fact]
    public async Task Signup_ErrorWhileCreatingUser_ReturnsBadRequest()
    {
        // Arrange
        var request = _authFixture.GetValidSignupRequest();
        var identityResult = _authFixture.GetIdentityResultFailed();
        _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(identityResult);

        // Act
        var result = await _controller.Signup(request);

        // Assert
        result.Should().BeOfType<ActionResult<IdDto>>();

        var resultObject = result.Result as BadRequestObjectResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as IEnumerable<IdentityError>;
        resultObjectValue?.FirstOrDefault()?.Code.Should().Be(identityResult.Errors.FirstOrDefault()?.Code);
    }

    [Fact]
    public async Task Signup_Success_ReturnsCreatedWithJwt()
    {
        // Arrange
        var request = _authFixture.GetValidSignupRequest();
        var user = _userFixture.GetValidUser();
        _userManagerMock.Setup(userManager => userManager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Act
        var result = await _controller.Signup(request);

        // Assert
        result.Should().BeOfType<ActionResult<IdDto>>();

        var resultObject = result.Result as CreatedResult;
        resultObject.Should().NotBeNull();

        var resultObjectValue = resultObject?.Value as IdDto;
        resultObjectValue?.Id.Should().NotBeNullOrEmpty().And.BeOfType<string>();

        _userManagerMock.ShouldHaveBeenCalled(manager => manager.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
    }
}