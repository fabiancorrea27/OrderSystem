namespace OrderSystem.API.Controllers;

using Microsoft.AspNetCore.Mvc;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.UseCases;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly RegisterUserUseCase _registerUseCase;
    private readonly LoginUseCase _loginUseCase;

    public AuthController(RegisterUserUseCase registerUseCase, LoginUseCase loginUseCase)
    {
        _registerUseCase = registerUseCase;
        _loginUseCase = loginUseCase;
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
}
