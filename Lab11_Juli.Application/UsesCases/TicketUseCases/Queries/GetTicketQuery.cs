using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.TicketUseCases.Queries;

public record GetTicketQuery() : IRequest<List<TicketDto>>;

internal class GetTicketsQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetTicketQuery, List<TicketDto>>
{
    public async Task<List<TicketDto>> Handle(GetTicketQuery request, CancellationToken cancellationToken)
    {
        var data = await unitOfWork.Repository<Ticket>().GetAll();
        return mapper.Map<List<TicketDto>>(data);
    }
}
