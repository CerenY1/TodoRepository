using Microsoft.EntityFrameworkCore;
using TodoApp.Backend.Entities.Entities;
using TodoApp.Backend.Entities.Interfaces;

namespace TodoApp.Backend.Infrastructure.Persistence;

public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;
    public ToDoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Todo>> GetAllAsync(Guid userId) 
    {
        return await _context.Todos.Where(t => t.UserId == userId).ToListAsync();
    }

    public async Task<Todo?> GetByIdAsync(Guid id) => await _context.Todos.FindAsync(id);
    
    public async Task AddAsync(Todo todo) 
    { 
        await _context.Todos.AddAsync(todo); 
        await _context.SaveChangesAsync(); 
    }
    
    public async Task UpdateAsync(Todo todo) 
    { 
        _context.Todos.Update(todo); 
        await _context.SaveChangesAsync(); 
    }
    
    public async Task DeleteAsync(Guid id) 
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo != null) { _context.Todos.Remove(todo); await _context.SaveChangesAsync(); }
    }

    public async Task<User> GetOrCreateUserAsync(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
        {
            user = new User { Id = Guid.NewGuid(), Email = email };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        return user;
    }
}