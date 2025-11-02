using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Application.UsesCases.RoleUseCases.Commands;
using Lab11_Juli.Application.UsesCases.RoleUseCases.Queries;
using Lab11_Juli.Domain.Ports.Repositories;
using Lab11_Juli.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_Juli.Controllers;

[ApiController]
[Route("api/Role")]
public class RoleController(IMediator mediator):ControllerBase
{
   [HttpGet]
   public async Task<IActionResult> GetAll()
   {
      var result = await mediator.Send(new GetRolesQuery());
      return Ok(result);
   }

   [HttpGet("{id}")]
   public async Task<IActionResult> GetById(Guid id)
   {
      var result = await mediator.Send(new GetRoleByIdQuery(id));
      return Ok(result);
   }

   [HttpPost]
   public async Task<IActionResult> Create([FromBody] RoleDto roleDto)
   {
      var result = await mediator.Send(new CreateRoleCommand(roleDto));
      return Ok(result);
   }

   [HttpPut("{id}")]
   public async Task<IActionResult> Update(Guid id, [FromBody] RoleDto roleDto)
   {
      var result = await mediator.Send(new UpdateRoleCommand(id, roleDto));
      return Ok(result);
   }

   [HttpDelete("{id}")]
   public async Task<IActionResult> Delete(Guid id)
   {
      var result = await mediator.Send(new DeleteRoleCommand(id));
      return Ok(result);
   }
}