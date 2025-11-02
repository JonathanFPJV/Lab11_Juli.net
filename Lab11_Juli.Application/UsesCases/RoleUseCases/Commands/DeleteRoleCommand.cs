using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.RoleUseCases.Commands;

public record DeleteRoleCommand(Guid RoleId) : IRequest<bool>;

internal sealed class DeleteRoleCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<DeleteRoleCommand, bool>
{
    

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await unitOfWork.Repository<Role>().GetById(request.RoleId);

        if (role == null)
            return false;

        await unitOfWork.Repository<Role>().Delete(request.RoleId);
        await unitOfWork.Complete();
        return true;
    }
}