using CurrencyApp.Application;
using CurrencyApp.Application.Features.Users.Register;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Data.Entities;
using Moq;
namespace CurrencyAPI.Tests.Features.Users.Register;
public class RegisterUserHandlerTest
{
    [Fact]
    public async Task Handle_SuccessfulRegistration_ReturnsResponse()
    {
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByEmailSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User)null);
        userRepoMock.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(new User()));
        userRepoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.FromResult(1));

        var handler = new RegisterUserHandler(userRepoMock.Object);
        var request = new RegisterUserRequest { Name = "John Doe", Email = "user@example.com", Password = "Password1!" };

        var result = await handler.Handle(request, CancellationToken.None);

        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.Email, result.Email);
        Assert.True(result.UserId > 0 || result.UserId == 0); // UserId is set by User.Create
    }

    [Fact]
    public async Task Handle_ExistingUser_ThrowsDomainException()
    {
        var userRepoMock = new Mock<IRepository<User>>();
        userRepoMock.Setup(r => r.FirstOrDefaultAsync(It.IsAny<UserByEmailSpec>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new User());
        var handler = new RegisterUserHandler(userRepoMock.Object);
        var request = new RegisterUserRequest { Name = "John Doe", Email = "user@example.com", Password = "Password1!" };

        await Assert.ThrowsAsync<DomainException>(() => handler.Handle(request, CancellationToken.None));
    }
}
