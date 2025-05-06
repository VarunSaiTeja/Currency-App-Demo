using CurrencyApp.Application;
using CurrencyApp.Application.Features.Users.Login;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Application.Services;
using CurrencyApp.Data.Entities;
using Moq;
namespace CurrencyAPI.Tests.Features.Users.Login;
public class LoginUserHandlerTest
{
    [Fact]
    public async Task Handle_SuccessfulLogin_ReturnsTokens()
    {
        // Arrange
        var user = User.Create("Test User", "test@example.com", "password123", new List<UserRole> { UserRole.Customer });
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByEmailSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        var refreshTokenRepoMock = new Mock<IRepository<RefreshToken>>();
        refreshTokenRepoMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new RefreshToken()));
        refreshTokenRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(s => s.GenerateAccessToken(It.IsAny<User>())).Returns("access-token");

        var handler = new LoginUserHandler(userRepoMock.Object, refreshTokenRepoMock.Object, tokenServiceMock.Object);
        var request = new LoginUserRequest { Email = "test@example.com", Password = "password123" };

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("access-token", result.AccessToken);
        Assert.False(string.IsNullOrEmpty(result.RefreshToken));
        Assert.True(result.AccessTokenExpiresOn > DateTime.UtcNow);
        Assert.True(result.RefreshTokenExpiresOn > DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_InvalidEmail_ThrowsDomainException()
    {
        // Arrange
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByEmailSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);
        var refreshTokenRepoMock = new Mock<IRepository<RefreshToken>>();
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginUserHandler(userRepoMock.Object, refreshTokenRepoMock.Object, tokenServiceMock.Object);
        var request = new LoginUserRequest { Email = "wrong@example.com", Password = "password123" };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_InvalidPassword_ThrowsDomainException()
    {
        // Arrange
        var user = User.Create("Test User", "test@example.com", "password123", new List<UserRole> { UserRole.Customer });
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByEmailSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        var refreshTokenRepoMock = new Mock<IRepository<RefreshToken>>();
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new LoginUserHandler(userRepoMock.Object, refreshTokenRepoMock.Object, tokenServiceMock.Object);
        var request = new LoginUserRequest { Email = "test@example.com", Password = "wrongpassword" };

        // Act & Assert
        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }
}
