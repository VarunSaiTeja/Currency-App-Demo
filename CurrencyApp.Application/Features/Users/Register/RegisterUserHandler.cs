using Ardalis.Specification;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Data.Entities;
using MediatR;

namespace CurrencyApp.Application.Features.Users.Register;

public class RegisterUserHandler(IRepository<User> userRepo) : IRequestHandler<RegisterUserRequest, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserRequest request, CancellationToken cancellationToken)
    {
        var user = User.Create(
            request.Name,
            request.Email,
            request.Password,
            [UserRole.Customer]
        );

        var existingUser = await userRepo.FirstOrDefaultAsync(new UserByEmailSpec(request.Email), cancellationToken);
        if (existingUser != null)
            throw new DomainException("User already exists");

        await userRepo.AddAsync(user, cancellationToken);
        await userRepo.SaveChangesAsync(cancellationToken);

        return new RegisterUserResponse
        {
            Name = user.Name,
            Email = user.Email,
            UserId = user.Id
        };
    }
}


public class UserByEmailSpec : SingleResultSpecification<User>
{
    public UserByEmailSpec(string email)
    {
        Query.Where(x => x.Email == email);
        Query.AsNoTracking();
    }
}