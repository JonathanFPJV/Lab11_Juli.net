using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Application.UsesCases.TicketUseCases.Commands;
using Lab11_Juli.Application.UsesCases.TicketUseCases.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_Juli.Configuration;

[ApiController]
[Route("api/[controller]")]
public class TicketController(IMediator mediator): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TicketDto>>> GetAll()
    {
        var result = await mediator.Send(new GetTicketQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TicketDto>> GetById(Guid id)
    {
        var result = await mediator.Send(new GetTicketByIdQuery(id));
        if (result is null)
            return NotFound($"Ticket con ID {id} no encontrado");

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TicketDto>> Create([FromBody] CreateTicketDto dto)
    {
        var result = await mediator.Send(new CreateTicketCommand(dto));
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTicketDto dto)
    {
        if (id != dto.TicketId)
            return BadRequest("El ID del ticket no coincide con el cuerpo de la solicitud.");

        var result = await mediator.Send(new UpdateTicketCommand(dto));
        if (!result)
            return NotFound($"No se encontró el ticket con ID {id}");

        return NoContent();
    }

    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await mediator.Send(new DeleteTicketCommand(id));
        if (!result)
            return NotFound($"No se encontró el ticket con ID {id}");

        return NoContent();
    }
}
