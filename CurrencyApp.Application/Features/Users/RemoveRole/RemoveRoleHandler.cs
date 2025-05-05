using CurrencyApp.Application.Persistence;
using CurrencyApp.Data.Entities;
using MediatR;

namespace CurrencyApp.Application.Features.Users.RemoveRole;

public class RemoveRoleHandler(IRepository<User> userRepo) : IRequestHandler<RemoveRoleRequest, RemoveRoleResponse>
{
    public async Task<RemoveRoleResponse> Handle(RemoveRoleRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepo.GetByIdAsync(request.UserId, cancellationToken) ?? throw new DomainException($"User with ID {request.UserId} not found.");
        if (!user.Roles.Contains(request.Role))
            throw new DomainException($"User does not have the role {request.Role}.");
        user.Roles.Remove(request.Role);

        await userRepo.SaveChangesAsync(cancellationToken);
        return new RemoveRoleResponse
        {
            UserId = user.Id,
            Roles = user.Roles
        };
    }
}
