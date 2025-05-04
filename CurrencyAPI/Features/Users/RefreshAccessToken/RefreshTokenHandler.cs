using Ardalis.Specification;
using CurrencyAPI.DAL.Entities;
using CurrencyAPI.Infra;
using MediatR;
using System.Security.Cryptography;

namespace CurrencyAPI.Features.Users.RefreshAccessToken;

public class RefreshTokenHandler(IRepository<RefreshToken> refreshTokenRepo, TokenService tokenService) : IRequestHandler<RefreshTokenRequest, RefreshTokenResponse>
{
    public async Task<RefreshTokenResponse> Handle(RefreshTokenRequest request, CancellationToken cancellationToken)
    {
        var refreshToken = await refreshTokenRepo.FirstOrDefaultAsync(new GetRefreshTokenSpec(request.RefreshToken), cancellationToken);

        if (refreshToken == null || refreshToken.ExpiresOn < DateTime.UtcNow)
            throw new DomainException("Invalid refresh token");

        var user = refreshToken.User;

        var newRefreshToken = RandomNumberGenerator.GetHexString(128).ToLower();
        var refreshTokenEntity = new RefreshToken
        {
            Token = newRefreshToken,
            UserId = user.Id,
            ExpiresOn = DateTime.UtcNow.AddDays(30)
        };

        await refreshTokenRepo.DeleteAsync(refreshToken, cancellationToken);
        await refreshTokenRepo.AddAsync(refreshTokenEntity, cancellationToken);
        await refreshTokenRepo.SaveChangesAsync(cancellationToken);

        return new RefreshTokenResponse
        {
            AccessToken = tokenService.GenerateAccessToken(user),
            RefreshToken = newRefreshToken,
            AccessTokenExpiresOn = DateTime.UtcNow.AddHours(1),
            RefreshTokenExpiresOn = DateTime.UtcNow.AddDays(30)
        };
    }
}

public class GetRefreshTokenSpec : SingleResultSpecification<RefreshToken>
{
    public GetRefreshTokenSpec(string token)
    {
        Query.Where(x => x.Token == token);
        Query.AsNoTracking();
        Query.Include(x => x.User);
        Query.AsSplitQuery();
    }
}