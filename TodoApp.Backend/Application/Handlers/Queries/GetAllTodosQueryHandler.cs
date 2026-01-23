using TodoApp.Backend.Domain;
using TodoApp.Backend.Application.Interfaces;

namespace TodoApp.Backend.Application.Handlers.Queries;

public class GetAllTodosQueryHandler
{
    private readonly IToDoRepository _repository;
    public GetAllTodosQueryHandler(IToDoRepository repository) => _repository = repository;
    public async Task<IEnumerable<Todo>> Handle() => await _repository.GetAllAsync();
}