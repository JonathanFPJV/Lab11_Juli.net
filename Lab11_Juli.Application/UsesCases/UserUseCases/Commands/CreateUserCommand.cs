using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Domain.Ports.Services;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.UserUseCases.Commands;

public record CreateUserCommand(RegisterUserDto UserDto) : IRequest<AuthResponseDto>;

internal sealed class CreateUserCommandHandler(IUnitOfWork unitOfWork,
IPasswordHasher passwordHasher,
IJwtTokenGenerator jwtTokenGenerator,
IMapper mapper)
: IRequestHandler<CreateUserCommand, AuthResponseDto>
{
    public async Task<AuthResponseDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = (await unitOfWork.Repository<User>().GetAll())
            .FirstOrDefault(u => u.Username == request.UserDto.Username);

        if (existingUser is not null)
            throw new Exception("El nombre de usuario ya está en uso.");

        // Buscar o crear el rol solicitado
        var roleRepo = unitOfWork.Repository<Role>();
        var userRoleRepo = unitOfWork.Repository<UserRole>();

        var role = (await roleRepo.GetAll())
            .FirstOrDefault(r => r.RoleName == request.UserDto.RoleName);

        if (role is null)
        {
            role = new Role { RoleId = Guid.NewGuid(), RoleName = request.UserDto.RoleName! };
            await roleRepo.Add(role);
        }

        // Crear el usuario
        var user = mapper.Map<User>(request.UserDto);
        user.UserId = Guid.NewGuid();
        user.PasswordHash = passwordHasher.Hash(request.UserDto.Password);

        await unitOfWork.Repository<User>().Add(user);

        // Relacionar el rol con el usuario
        var userRole = new UserRole
        {
            UserId = user.UserId,
            RoleId = role.RoleId,
            AssignedAt = DateTime.Now
        };
        await userRoleRepo.Add(userRole);

        await unitOfWork.Complete();

        // Generar token JWT
        var token = jwtTokenGenerator.GenerateToken(user, new[] { role.RoleName });

        var response = mapper.Map<AuthResponseDto>(user);
        response.Role = role.RoleName;
        response.Token = token;

        return response;
    }
}