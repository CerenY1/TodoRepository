using Microsoft.AspNetCore.Mvc;
using TodoApp.Backend.Domain;
using TodoApp.Backend.Application.Handlers.Queries; 
using TodoApp.Backend.Application.Handlers.Commands; 

namespace TodoApp.Backend.Application.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly GetAllTodosQueryHandler _getAllHandler;
    private readonly GetTodoByIdQueryHandler _getByIdHandler;
    private readonly CreateTodoCommandHandler _createHandler;
    private readonly UpdateTodoCommandHandler _updateHandler;
    private readonly DeleteTodoCommandHandler _deleteHandler;

    public TodoController(
        GetAllTodosQueryHandler getAllHandler, 
        GetTodoByIdQueryHandler getByIdHandler,
        CreateTodoCommandHandler createHandler,
        UpdateTodoCommandHandler updateHandler,
        DeleteTodoCommandHandler deleteHandler)
    {
        _getAllHandler = getAllHandler;
        _getByIdHandler = getByIdHandler;
        _createHandler = createHandler;
        _updateHandler = updateHandler;
        _deleteHandler = deleteHandler;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _getAllHandler.Handle());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id) 
    {
        var todo = await _getByIdHandler.Handle(id);
        return todo == null ? NotFound() : Ok(todo);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Todo todo) 
    {
        await _createHandler.Handle(todo);
        return Ok(todo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] Todo todo)
    {
        if (id != todo.Id) return BadRequest();
        await _updateHandler.Handle(todo);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _deleteHandler.Handle(id);
        return NoContent();
    }
}