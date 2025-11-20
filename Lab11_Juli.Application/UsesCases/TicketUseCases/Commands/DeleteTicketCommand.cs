using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.TicketUseCases.Commands;

public record DeleteTicketCommand(Guid TicketId) : IRequest<bool>;

internal class DeleteTicketCommandHandler(IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteTicketCommand, bool>
{
    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await unitOfWork.Repository<Ticket>().GetById(request.TicketId);
        if (ticket is null) return false;

        await unitOfWork.Repository<Ticket>().Delete(request.TicketId);
        await unitOfWork.Complete();
        return true;
    }
}