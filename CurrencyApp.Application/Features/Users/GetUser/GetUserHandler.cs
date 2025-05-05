using Ardalis.Specification;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Data.Entities;
using FluentValidation;
using MediatR;

namespace CurrencyApp.Application.Features.Users.GetUser;

public class GetUserHandler(IRepository<User> userRepo) : IRequestHandler<GetUserRequest, GetUserResponse>
{
    public async Task<GetUserResponse> Handle(GetUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepo.FirstOrDefaultAsync(new UserByEmailSpec(request.Email), cancellationToken)
            ?? throw new DomainException("User not found");
        return new GetUserResponse
        {
            Id = user.Id,
            Email = user.Email,
            Name = user.Name,
            Roles = user.Roles
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