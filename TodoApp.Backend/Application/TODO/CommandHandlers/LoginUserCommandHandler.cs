using MediatR;
using TodoApp.Backend.Entities.Interfaces;

namespace TodoApp.Backend.Application.TodoApp.CommandHandlers;

public record LoginUserCommand(string Email) : IRequest<Guid>;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Guid>
{
    private readonly IToDoRepository _repository;
    public LoginUserCommandHandler(IToDoRepository repository) => _repository = repository;

    public async Task<Guid> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetOrCreateUserAsync(request.Email);
        return user.Id;
    }
}