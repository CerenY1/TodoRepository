using MediatR;
using TodoApp.Backend.Entities.Interfaces;

namespace TodoApp.Backend.Application.TodoApp.CommandHandlers;

public class LoginUserCommand : IRequest<Guid>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Guid>
{
    private readonly IToDoRepository _repository;
    
    public LoginUserCommandHandler(IToDoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Guid> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _repository.GetUserByEmailAsync(request.Email);

        if (user == null || user.Password != request.Password)
        {
            return Guid.Empty; 
        }

        return user.Id;
    }
}