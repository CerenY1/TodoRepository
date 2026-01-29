using MediatR;
using TodoApp.Backend.Entities.ItemDtos;
using TodoApp.Backend.Entities.Interfaces;

namespace TodoApp.Backend.Application.TodoApp.QueryHandlers;

public record GetAllTodosQuery(Guid UserId) : IRequest<List<TodoItemDto>>;

public class GetAllTodosQueryHandler : IRequestHandler<GetAllTodosQuery, List<TodoItemDto>>
{
    private readonly IToDoRepository _repository;
    public GetAllTodosQueryHandler(IToDoRepository repository) => _repository = repository;

    public async Task<List<TodoItemDto>> Handle(GetAllTodosQuery request, CancellationToken cancellationToken)
    {
        var entities = await _repository.GetAllAsync(request.UserId);

        return entities.Select(x => new TodoItemDto
        {
            Id = x.Id,
            Title = x.Title,
            Description = x.Description,
            IsCompleted = x.IsCompleted
        }).ToList();
    }
}