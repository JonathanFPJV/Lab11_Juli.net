using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.RoleUseCases.Queries;

public record GetRoleByIdQuery(Guid RoleId) : IRequest<Role?>;

internal sealed class GetRoleByIdQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetRoleByIdQuery, Role?>
{
  public async Task<Role?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
  {
    return await unitOfWork.Repository<Role>().GetById(request.RoleId);
  }  
}