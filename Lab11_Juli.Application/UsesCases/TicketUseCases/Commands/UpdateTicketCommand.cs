using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.TicketUseCases.Commands;

public record UpdateTicketCommand(UpdateTicketDto Ticket) : IRequest<bool>;

internal class UpdateTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<UpdateTicketCommand, bool>
{
    public async Task<bool> Handle(UpdateTicketCommand request, CancellationToken cancellationToken)
    {
        var existing = await unitOfWork.Repository<Ticket>().GetById(request.Ticket.TicketId);
        if (existing is null) return false;

        mapper.Map(request.Ticket, existing);

        await unitOfWork.Repository<Ticket>().Update(existing);
        await unitOfWork.Complete();

        return true;
    }
}