using MediatR;
using TodoApp.Backend.Entities.Interfaces;

namespace TodoApp.Backend.Application.TodoApp.CommandHandlers;

public record DeleteTodoCommand(Guid Id) : IRequest<bool>;

public class DeleteTodoCommandHandler : IRequestHandler<DeleteTodoCommand, bool>
{
    private readonly IToDoRepository _repository;

    public DeleteTodoCommandHandler(IToDoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTodoCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id); // 1. İşlemi yap
        
        return true; 
    }
}