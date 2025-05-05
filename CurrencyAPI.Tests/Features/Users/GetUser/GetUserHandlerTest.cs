using CurrencyApp.Application;
using CurrencyApp.Application.Features.Users.GetUser;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Data.Entities;
using Moq;
namespace CurrencyAPI.Tests.Features.Users.GetUser;
public class GetUserHandlerTest
{
    [Fact]
    public async Task Handle_UserExists_ReturnsUserResponse()
    {
        var user = new User { Id = 1, Name = "Test User", Email = "test@example.com", Roles = new List<UserRole> { UserRole.Customer } };
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByEmailSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);
        var handler = new GetUserHandler(userRepoMock.Object);
        var request = new GetUserRequest { Email = "test@example.com" };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Name, result.Name);
        Assert.Equal(user.Email, result.Email);
        Assert.Equal(user.Roles, result.Roles);
    }

    [Fact]
    public async Task Handle_UserDoesNotExist_ThrowsDomainException()
    {
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByEmailSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);
        var handler = new GetUserHandler(userRepoMock.Object);
        var request = new GetUserRequest { Email = "notfound@example.com" };

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }
}
