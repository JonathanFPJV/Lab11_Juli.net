using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.RoleUseCases.Queries;

public record GetRolesQuery(): IRequest<List<Role>>;

internal sealed class GetRolesQueryHandler (IUnitOfWork unitOfWork) : IRequestHandler<GetRolesQuery, List<Role>>
{
    public async Task<List<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var data = await unitOfWork.Repository<Role>().GetAll();
        return data;
    }
}