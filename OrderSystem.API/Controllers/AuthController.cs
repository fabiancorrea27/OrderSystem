namespace OrderSystem.API.Controllers;

using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.UseCases;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUseCase;
    private readonly LoginUseCase _loginUseCase;
    private readonly GetProfileUseCase _getProfileUseCase;

    public AuthController(
        RegisterUserUseCase registerUseCase,
        LoginUseCase loginUseCase,
        GetProfileUseCase getProfileUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
        _getProfileUseCase = getProfileUseCase;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        try
        {
            await _registerUseCase.Execute(dto);
            return Ok(new { message = "User registered successfully." });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await _loginUseCase.Execute(dto);
        if (token is null)
            return Unauthorized(new { error = "Invalid email or password." });

        return Ok(new { token });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();

        var profile = await _getProfileUseCase.Execute(userId);
        if (profile is null)
            return NotFound(new { error = "User not found." });

        return Ok(profile);
    }
}
