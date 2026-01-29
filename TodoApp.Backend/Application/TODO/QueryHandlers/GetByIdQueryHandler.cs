using MediatR;
using TodoApp.Backend.Entities.ItemDtos;
using TodoApp.Backend.Entities.Interfaces;

namespace TodoApp.Backend.Application.TodoApp.QueryHandlers;

public record GetTodoByIdQuery(Guid Id) : IRequest<TodoItemDto?>;

public class GetTodoByIdQueryHandler : IRequestHandler<GetTodoByIdQuery, TodoItemDto?>
{
    private readonly IToDoRepository _repository;
    public GetTodoByIdQueryHandler(IToDoRepository repository) => _repository = repository;

    public async Task<TodoItemDto?> Handle(GetTodoByIdQuery request, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetByIdAsync(request.Id);
        if (todo == null) return null;

        return new TodoItemDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted
        };
    }
}