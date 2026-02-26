using Insurance.Application.Abstractions.Repositories;
using Insurance.Application.Authentication;
using Insurance.Application.Authentication.Login;
using Insurance.Application.Exceptions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Insurance.Tests.Unit.Auth
{
    public class LoginCommandHandlerTests
    {
        [Fact]
        public async Task Handle_WithValidCredentials_ReturnsAccessToken_AndCallsTokenGeneratorWithExpectedContext()
        {
            var username = "user1";
            var password = "Pa$$w0rd";
            var userId = Guid.NewGuid();
            var role = "User";
            Guid? brokerId = null;
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new UserAuthData(userId, username, passwordHash, role, brokerId);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock
                .Setup(r => r.GetByUsernameAsync(username, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var expectedToken = "jwt-token";
            var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            tokenGeneratorMock
                .Setup(g => g.Generate(It.IsAny<AuthUserContext>()))
                .Returns(expectedToken);

            var handler = new LoginCommandHandler(userRepoMock.Object, tokenGeneratorMock.Object);
            var command = new LoginCommand(username, password);

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(expectedToken, result.AccessToken);
            tokenGeneratorMock.Verify(g =>
                g.Generate(It.Is<AuthUserContext>(ctx =>
                    ctx.UserId == userId &&
                    ctx.Username == username &&
                    ctx.Role == role &&
                    ctx.BrokerId == brokerId
                )),
                Times.Once);
        }

        [Fact]
        public async Task Handle_UserNotFound_ThrowsUnauthorizedException()
        {
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock
                .Setup(r => r.GetByUsernameAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserAuthData?)null);

            var handler = new LoginCommandHandler(userRepoMock.Object, Mock.Of<IJwtTokenGenerator>());

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                handler.Handle(new LoginCommand("unknown", "pw"), CancellationToken.None));
        }

        [Fact]
        public async Task Handle_InvalidPassword_ThrowsUnauthorizedException()
        {
            var username = "user1";
            var correctPassword = "correct";
            var wrongPassword = "wrong";
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(correctPassword);

            var user = new UserAuthData(Guid.NewGuid(), username, passwordHash, "User", null);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock
                .Setup(r => r.GetByUsernameAsync(username, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var handler = new LoginCommandHandler(userRepoMock.Object, Mock.Of<IJwtTokenGenerator>());

            await Assert.ThrowsAsync<UnauthorizedException>(() =>
                handler.Handle(new LoginCommand(username, wrongPassword), CancellationToken.None));
        }

        [Fact]
        public async Task Handle_BrokerRole_IncludesBrokerIdInAuthUserContext()
        {
            var username = "broker1";
            var password = "brokerpw";
            var brokerId = Guid.NewGuid();
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new UserAuthData(Guid.NewGuid(), username, passwordHash, "Broker", brokerId);

            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock
                .Setup(r => r.GetByUsernameAsync(username, It.IsAny<CancellationToken>()))
                .ReturnsAsync(user);

            var expectedToken = "broker-token";
            var tokenGeneratorMock = new Mock<IJwtTokenGenerator>();
            tokenGeneratorMock
                .Setup(g => g.Generate(It.Is<AuthUserContext>(ctx => ctx.BrokerId == brokerId)))
                .Returns(expectedToken);

            var handler = new LoginCommandHandler(userRepoMock.Object, tokenGeneratorMock.Object);

            var result = await handler.Handle(new LoginCommand(username, password), CancellationToken.None);

            Assert.Equal(expectedToken, result.AccessToken);
            tokenGeneratorMock.Verify(g =>
                g.Generate(It.Is<AuthUserContext>(ctx => ctx.BrokerId == brokerId && ctx.Role == "Broker")),
                Times.Once);
        }
    }
}
