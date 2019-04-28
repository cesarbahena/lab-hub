using QuimiosHub.Models;

namespace QuimiosHub.Services;

public interface IAuthService
{
    Task<LoginResponse?> AuthenticateAsync(string username, string password);
    Task<User?> ValidateTokenAsync(string token);
}
