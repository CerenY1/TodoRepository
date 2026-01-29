using MediatR;
using TodoApp.Backend.Entities.Interfaces; // Context yerine Interface

namespace TodoApp.Backend.Application.TodoApp.CommandHandlers;

public class UpdateTodoCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid UserId { get; set; }
}

public class UpdateTodoCommandHandler : IRequestHandler<UpdateTodoCommand, bool>
{
    private readonly IToDoRepository _repository; // Context yerine Repository

    public UpdateTodoCommandHandler(IToDoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(UpdateTodoCommand request, CancellationToken cancellationToken)
    {
        var todo = await _repository.GetByIdAsync(request.Id);
        if (todo == null) return false;

        todo.Title = request.Title;
        todo.Description = request.Description;
        todo.IsCompleted = request.IsCompleted;
        
        await _repository.UpdateAsync(todo);
        return true;
    }
}