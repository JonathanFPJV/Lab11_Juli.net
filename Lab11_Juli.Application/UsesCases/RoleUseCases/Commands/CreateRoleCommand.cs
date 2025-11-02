using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.RoleUseCases.Commands;

public record CreateRoleCommand(RoleDto RoleDto) : IRequest<Role>;

internal sealed class CreateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateRoleCommand, Role>
{
    public async Task<Role> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = mapper.Map<Role>(request.RoleDto);
        await unitOfWork.Repository<Role>().Add(role);
        await unitOfWork.Complete();
        return role;
    }
}