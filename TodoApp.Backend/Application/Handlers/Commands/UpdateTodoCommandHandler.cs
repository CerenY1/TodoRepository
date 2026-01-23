using TodoApp.Backend.Domain;
using TodoApp.Backend.Application.Interfaces;

namespace TodoApp.Backend.Application.Handlers.Commands;

public class UpdateTodoCommandHandler
{
    private readonly IToDoRepository _repository;
    public UpdateTodoCommandHandler(IToDoRepository repository) => _repository = repository;
    public async Task Handle(Todo todo) => await _repository.UpdateAsync(todo);
}