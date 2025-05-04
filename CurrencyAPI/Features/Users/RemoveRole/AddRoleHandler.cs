using CurrencyAPI.DAL.Entities;
using CurrencyAPI.Features.Users.RemoveRole;
using CurrencyAPI.Infra;
using MediatR;

namespace CurrencyAPI.Features.Users.AddRole;

public class RemoveRoleHandler(IRepository<User> userRepo) : IRequestHandler<RemoveRoleRequest, RemoveRoleResponse>
{
    public async Task<RemoveRoleResponse> Handle(RemoveRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByIdAsync(request.UserId, cancellationToken) ?? throw new DomainException($"User with ID {request.UserId} not found.");
        user.Roles.Remove(request.Role);

        await userRepo.SaveChangesAsync(cancellationToken);
        return new RemoveRoleResponse
        {
            UserId = user.Id,
            Roles = user.Roles
        };
    }
}
