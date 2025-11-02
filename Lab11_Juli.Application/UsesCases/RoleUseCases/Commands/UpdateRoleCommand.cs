using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.RoleUseCases.Commands;

public record UpdateRoleCommand(Guid RoleId, RoleDto RoleDto) : IRequest<Role?>;


internal sealed class UpdateRoleCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<UpdateRoleCommand, Role?>
{
    public async Task<Role?> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Repository<Role>().GetById(request.RoleId);

        if (existing == null)
            return null;

        mapper.Map(request.RoleDto, existing);
        await unitOfWork.Repository<Role>().Update(existing);
        await unitOfWork.Complete();

        return existing;
    }
}
