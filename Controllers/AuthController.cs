using Microsoft.AspNetCore.Mvc;
using QuimiosHub.Models;
using QuimiosHub.Services;

namespace QuimiosHub.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.AuthenticateAsync(request.Username, request.Password);

        if (result == null)
            return Unauthorized(new { message = "Invalid username or password" });

        return Ok(result);
    }

    [HttpPost("validate")]
    public async Task<ActionResult> ValidateToken([FromHeader(Name = "Authorization")] string? authorization)
    {
        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            return Unauthorized(new { message = "Missing or invalid authorization header" });

        var token = authorization.Substring("Bearer ".Length).Trim();
        var user = await _authService.ValidateTokenAsync(token);

        if (user == null)
            return Unauthorized(new { message = "Invalid or expired token" });

        return Ok(new
        {
            username = user.Username,
            fullName = user.FullName,
            role = user.Role
        });
    }
}
