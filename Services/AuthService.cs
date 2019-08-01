using Microsoft.EntityFrameworkCore;
using QuimiOSHub.Data;
using QuimiOSHub.Models;
using System.Security.Cryptography;
using System.Text;

namespace QuimiOSHub.Services;

public class AuthService : IAuthService
{
    private readonly QuimiosDbContext _context;
    private static readonly Dictionary<string, (int UserId, DateTime ExpiresAt)> _tokenStore = new();

    public AuthService(QuimiosDbContext context)
    {
        _context = context;
    }

    public async Task<LoginResponse?> AuthenticateAsync(string username, string password)
    {
        var user = await _context.Users
            .Where(u => u.Username == username && u.IsActive)
            .FirstOrDefaultAsync();

        if (user == null)
            return null;

        if (!VerifyPassword(password, user.PasswordHash))
            return null;

        var token = GenerateToken();
        var expiresAt = DateTime.UtcNow.AddHours(8);

        _tokenStore[token] = (user.Id, expiresAt);

        return new LoginResponse
        {
            Token = token,
            Username = user.Username,
            FullName = user.FullName,
            Role = user.Role,
            ExpiresAt = expiresAt
        };
    }

    public async Task<User?> ValidateTokenAsync(string token)
    {
        if (!_tokenStore.TryGetValue(token, out var tokenData))
            return null;

        if (tokenData.ExpiresAt < DateTime.UtcNow)
        {
            _tokenStore.Remove(token);
            return null;
        }

        return await _context.Users
            .Where(u => u.Id == tokenData.UserId && u.IsActive)
            .FirstOrDefaultAsync();
    }

    private string GenerateToken()
    {
        var randomBytes = new byte[32];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }
        return Convert.ToBase64String(randomBytes);
    }

    public static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return Convert.ToBase64String(hashedBytes);
    }

    private bool VerifyPassword(string password, string passwordHash)
    {
        var hashOfInput = HashPassword(password);
        return hashOfInput == passwordHash;
    }
}
