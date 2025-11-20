using AutoMapper;
using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;

namespace Lab11_Juli.Application.UsesCases.TicketUseCases.Commands;

public record CreateTicketCommand(CreateTicketDto Ticket) : IRequest<CreateTicketDto>;

internal class CreateTicketCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    : IRequestHandler<CreateTicketCommand, CreateTicketDto>
{
    public async Task<CreateTicketDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Ticket>(request.Ticket);
        entity.TicketId = Guid.NewGuid();
        entity.CreatedAt = DateTime.Now;

        await unitOfWork.Repository<Ticket>().AddAsync(entity);
        await unitOfWork.Complete();

        return mapper.Map<CreateTicketDto>(entity);
    }
}