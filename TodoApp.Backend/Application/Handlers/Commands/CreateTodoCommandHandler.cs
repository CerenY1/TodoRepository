using TodoApp.Backend.Domain;
using TodoApp.Backend.Application.Interfaces;

namespace TodoApp.Backend.Application.Handlers.Commands;

public class CreateTodoCommandHandler
{
    private readonly IToDoRepository _repository;
    public CreateTodoCommandHandler(IToDoRepository repository) => _repository = repository;

    public async Task Handle(Todo todo) => await _repository.AddAsync(todo);
}