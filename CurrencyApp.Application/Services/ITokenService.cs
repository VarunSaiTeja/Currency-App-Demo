using CurrencyApp.Data.Entities;

namespace CurrencyApp.Application.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
}