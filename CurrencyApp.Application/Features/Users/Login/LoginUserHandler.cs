using Ardalis.Specification;
using CurrencyApp.Application.Persistence;
using CurrencyApp.Application.Services;
using CurrencyApp.Data.Entities;
using MediatR;
using System.Security.Cryptography;

namespace CurrencyApp.Application.Features.Users.Login;

public class LoginUserHandler(
    IRepository<User> userRepo,
    IRepository<RefreshToken> refreshTokenRepo,
    ITokenService tokenService) : IRequestHandler<LoginUserRequest, LoginUserResponse>
{
    public async Task<LoginUserResponse> Handle(LoginUserRequest request, CancellationToken cancellationToken)
    {
        var user = await userRepo.FirstOrDefaultAsync(new UserByEmailSpec(request.Email), cancellationToken);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            throw new DomainException("Invalid email or password");

        var refreshToken = RandomNumberGenerator.GetHexString(128).ToLower();
        var refreshTokenEntity = new RefreshToken
        {
            Token = refreshToken,
            UserId = user.Id,
            ExpiresOn = DateTime.UtcNow.AddDays(30)
        };

        await refreshTokenRepo.AddAsync(refreshTokenEntity, cancellationToken);
        await refreshTokenRepo.SaveChangesAsync(cancellationToken);

        return new LoginUserResponse
        {
            AccessToken = tokenService.GenerateAccessToken(user),
            RefreshToken = refreshToken,
            AccessTokenExpiresOn = DateTime.UtcNow.AddHours(1),
            RefreshTokenExpiresOn = DateTime.UtcNow.AddDays(30)
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