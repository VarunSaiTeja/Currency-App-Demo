using CurrencyAPI.DAL.Entities;
using CurrencyAPI.Infra;
using MediatR;

namespace CurrencyAPI.Features.Users.AddRole;

public class AddRoleHandler(IRepository<User> userRepo) : IRequestHandler<AddRoleRequest, AddRoleResponse>
{
    public async Task<AddRoleResponse> Handle(AddRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByIdAsync(request.UserId, cancellationToken) ?? throw new DomainException($"User with ID {request.UserId} not found.");
        user.Roles = [.. user.Roles, request.Role];

        await userRepo.SaveChangesAsync(cancellationToken);
        return new AddRoleResponse
        {
            UserId = user.Id,
            Roles = user.Roles
        };
    }
}
