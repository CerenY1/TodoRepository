using MediatR;
using TodoApp.Backend.Entities.Entities;
using TodoApp.Backend.Entities.Interfaces;

namespace TodoApp.Backend.Application.TodoApp.CommandHandlers;

public record CreateTodoCommand(string Title, string? Description, Guid UserId) : IRequest<Guid>;

public class CreateTodoCommandHandler : IRequestHandler<CreateTodoCommand, Guid>
{
    private readonly IToDoRepository _repository;
    public CreateTodoCommandHandler(IToDoRepository repository) => _repository = repository;

    public async Task<Guid> Handle(CreateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = new Todo
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            IsCompleted = false,
            CreatedAt = DateTime.Now,
            UserId = request.UserId
        };

        await _repository.AddAsync(todo);
        return todo.Id;
    }
}