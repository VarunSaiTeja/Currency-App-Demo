using CurrencyApp.Application;
using CurrencyApp.Application.Features.Users.RemoveRole;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Data.Entities;
using Moq;
namespace CurrencyAPI.Tests.Features.Users.RemoveRole;
public class RemoveRoleHandlerTest
{
    [Fact]
    public async Task Handle_UserExistsAndHasRole_RemovesRoleAndReturnsResponse()
    {
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com", Roles = new List<UserRole> { UserRole.Admin, UserRole.Customer } };
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        userRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).Returns(Task.FromResult(1));
        var handler = new RemoveRoleHandler(userRepoMock.Object);
        var request = new RemoveRoleRequest { UserId = 1, Role = UserRole.Admin };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(user.Id, result.UserId);
        Assert.DoesNotContain(UserRole.Admin, result.Roles);
        userRepoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_UserNotFound_ThrowsDomainException()
    {
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync((User)null);
        var handler = new RemoveRoleHandler(userRepoMock.Object);
        var request = new RemoveRoleRequest { UserId = 1, Role = UserRole.Admin };

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }

    [Fact]
    public async Task Handle_UserDoesNotHaveRole_ThrowsDomainException()
    {
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com", Roles = new List<UserRole> { UserRole.Customer } };
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.GetByIdAsync(1, It.IsAny<CancellationToken>())).ReturnsAsync(user);
        var handler = new RemoveRoleHandler(userRepoMock.Object);
        var request = new RemoveRoleRequest { UserId = 1, Role = UserRole.Admin };

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }
}
