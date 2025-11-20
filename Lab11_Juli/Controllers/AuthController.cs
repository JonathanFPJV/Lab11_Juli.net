using Lab11_Juli.Application.DTOs;
using Lab11_Juli.Application.UsesCases.UserUseCases.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_Juli.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto dto)
    {
        var result = await mediator.Send(new CreateUserCommand(dto));
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto dto)
    {
        var result = await mediator.Send(new LoginUserCommand(dto));
        return Ok(result);
    }
}