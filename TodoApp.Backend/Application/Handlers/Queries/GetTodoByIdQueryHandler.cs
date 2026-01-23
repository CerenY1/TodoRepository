using TodoApp.Backend.Domain;
using TodoApp.Backend.Application.Interfaces;

namespace TodoApp.Backend.Application.Handlers.Queries;

public class GetTodoByIdQueryHandler
{
    private readonly IToDoRepository _repository;
    public GetTodoByIdQueryHandler(IToDoRepository repository) => _repository = repository;
    public async Task<Todo?> Handle(Guid id) => await _repository.GetByIdAsync(id);
}