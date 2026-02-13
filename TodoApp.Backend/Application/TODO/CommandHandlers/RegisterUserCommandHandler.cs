using MediatR;
using Microsoft.EntityFrameworkCore; 
using TodoApp.Backend.Infrastructure.Persistence; 
using TodoApp.Backend.Entities.Entities;

namespace TodoApp.Backend.Application.TodoApp.CommandHandlers;

public class RegisterUserCommand : IRequest<bool>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, bool>
{
    private readonly AppDbContext _context; 

    public RegisterUserCommandHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {

        if (await _context.Users.AnyAsync(u => u.Email == request.Email, cancellationToken)) 
        {
            return false; 
        }

        var newUser = new User 
        { 
            Id = Guid.NewGuid(), 
            Email = request.Email, 
            Password = request.Password 
        };

        _context.Users.Add(newUser);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}