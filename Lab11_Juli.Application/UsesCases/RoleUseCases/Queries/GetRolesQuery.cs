using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.RoleUseCases.Queries;

public record GetRolesQuery(): IRequest<List<RoleGetDto>>;

internal sealed class GetRolesQueryHandler (IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<GetRolesQuery, List<RoleGetDto>>
{
    public async Task<List<RoleGetDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var data = await unitOfWork.Repository<Role>().GetAll();
        return mapper.Map<List<RoleGetDto>>(data) ;
    }
}