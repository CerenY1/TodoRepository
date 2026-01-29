using MediatR;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Backend.Application.TodoApp.CommandHandlers;
using TodoApp.Backend.Application.TodoApp.QueryHandlers;

namespace TodoApp.Backend.Application.TodoApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly IMediator _mediator;

    public TodoController(IMediator mediator) => _mediator = mediator;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid userId)
    {
        return Ok(await _mediator.Send(new GetAllTodosQuery(userId)));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTodoCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateTodoCommand command)
    {
        return Ok(await _mediator.Send(command));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        return Ok(await _mediator.Send(new DeleteTodoCommand(id)));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetTodoByIdQuery(id)); // Yukarıda oluşturduğumuz Query
        if (result == null) return NotFound();
        return Ok(result);
    }
}