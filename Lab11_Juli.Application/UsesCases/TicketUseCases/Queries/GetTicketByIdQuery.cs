using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.TicketUseCases.Queries;

public record GetTicketByIdQuery(Guid TicketId) : IRequest<TicketDto?>;

internal class GetTicketByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<GetTicketByIdQuery, TicketDto?>
{
    public async Task<TicketDto?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        var ticket = await unitOfWork.Repository<Ticket>().GetById(request.TicketId);
        return mapper.Map<TicketDto?>(ticket);
    }
}