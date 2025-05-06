using CurrencyApp.Application;
using CurrencyApp.Application.Features.Users.RefreshAccessToken;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Application.Services;
using CurrencyApp.Data.Entities;
using Moq;
namespace CurrencyAPI.Tests.Features.Users.RefreshUserToken;
public class RefreshTokenHandlerTest
{
    [Fact]
    public async Task Handle_ValidRefreshToken_ReturnsNewTokens()
    {
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com", Roles = new List<UserRole> { UserRole.Customer } };
        var oldRefreshToken = new RefreshToken { Token = "oldtoken", UserId = 1, User = user, ExpiresOn = DateTime.UtcNow.AddDays(1) };
        var refreshTokenRepoMock = new Mock<IRepository<RefreshToken>>();
        refreshTokenRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetRefreshTokenSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(oldRefreshToken);
        refreshTokenRepoMock.Setup(r => r.DeleteAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(0));
        refreshTokenRepoMock.Setup(r => r.AddAsync(It.IsAny<RefreshToken>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new RefreshToken()));
        refreshTokenRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));
        var tokenServiceMock = new Mock<ITokenService>();
        tokenServiceMock.Setup(s => s.GenerateAccessToken(It.IsAny<User>())).Returns("access-token");
        var handler = new RefreshTokenHandler(refreshTokenRepoMock.Object, tokenServiceMock.Object);
        var request = new RefreshTokenRequest { RefreshToken = "oldtoken" };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal("access-token", result.AccessToken);
        Assert.False(string.IsNullOrEmpty(result.RefreshToken));
        Assert.True(result.AccessTokenExpiresOn > DateTime.UtcNow);
        Assert.True(result.RefreshTokenExpiresOn > DateTime.UtcNow);
    }

    [Fact]
    public async Task Handle_InvalidOrExpiredToken_ThrowsDomainException()
    {
        var refreshTokenRepoMock = new Mock<IRepository<RefreshToken>>();
        refreshTokenRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetRefreshTokenSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((RefreshToken)null);
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new RefreshTokenHandler(refreshTokenRepoMock.Object, tokenServiceMock.Object);
        var request = new RefreshTokenRequest { RefreshToken = "invalidtoken" };

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_ExpiredToken_ThrowsDomainException()
    {
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com", Roles = new List<UserRole> { UserRole.Customer } };
        var expiredToken = new RefreshToken { Token = "expiredtoken", UserId = 1, User = user, ExpiresOn = DateTime.UtcNow.AddDays(-1) };
        var refreshTokenRepoMock = new Mock<IRepository<RefreshToken>>();
        refreshTokenRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<GetRefreshTokenSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expiredToken);
        var tokenServiceMock = new Mock<ITokenService>();
        var handler = new RefreshTokenHandler(refreshTokenRepoMock.Object, tokenServiceMock.Object);
        var request = new RefreshTokenRequest { RefreshToken = "expiredtoken" };

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }
}
