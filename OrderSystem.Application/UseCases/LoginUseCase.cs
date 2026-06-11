namespace OrderSystem.Application.UseCases;

using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Interfaces;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtService _jwtService;

    public LoginUseCase(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<string?> Execute(LoginDto dto)
    {
        var user = await _userRepository.GetByEmail(dto.Email);
        if (user is null)
            return null;

        var valid = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
        if (!valid)
            return null;

        return _jwtService.GenerateToken(user);
    }
}
