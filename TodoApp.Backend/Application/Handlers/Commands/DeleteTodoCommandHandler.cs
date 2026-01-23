// Application/Handlers/Commands/DeleteTodoCommandHandler.cs
using TodoApp.Backend.Application.Interfaces;

namespace TodoApp.Backend.Application.Handlers.Commands;

public class DeleteTodoCommandHandler
{
    private readonly IToDoRepository _repository;
    public DeleteTodoCommandHandler(IToDoRepository repository) => _repository = repository;
    public async Task Handle(Guid id) 
    {
        await _repository.DeleteAsync(id);
    }
}