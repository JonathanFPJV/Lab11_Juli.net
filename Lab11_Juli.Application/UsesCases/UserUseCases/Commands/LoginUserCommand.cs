using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Domain.Ports.Services;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.UserUseCases.Commands;

public record LoginUserCommand(LoginUserDto UserDto) : IRequest<AuthResponseDto>;

internal sealed class LoginUserCommandHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator,
    IMapper mapper)
    : IRequestHandler<LoginUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var users = await unitOfWork.Repository<User>().GetAll();
        var user = users.FirstOrDefault(u => u.Username == request.UserDto.Username);

        if (user is null)
            throw new Exception("Usuario no encontrado.");

        if (!passwordHasher.Verify(user.PasswordHash, request.UserDto.Password))
            throw new Exception("Contraseña incorrecta.");

        // Obtener roles del usuario
        var userRoles = await unitOfWork.Repository<UserRole>().GetAll();
        var roles = userRoles
            .Where(ur => ur.UserId == user.UserId)
            .Select(ur => ur.Role.RoleName)
            .ToList();

        var token = jwtTokenGenerator.GenerateToken(user, roles);

        var response = mapper.Map<AuthResponseDto>(user);
        response.Role = string.Join(",", roles);
        response.Token = token;

        return response;
    }
}