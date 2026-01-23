using Microsoft.EntityFrameworkCore;
using TodoApp.Backend.Domain;
using TodoApp.Backend.Application.Interfaces;

namespace TodoApp.Backend.Infrasturcture.Persistance;

public class ToDoRepository : IToDoRepository
{
    private readonly AppDbContext _context;
    public ToDoRepository(AppDbContext context) => _context = context;

    public async Task<IEnumerable<Todo>> GetAllAsync() => await _context.ToDo.ToListAsync();
    public async Task<Todo?> GetByIdAsync(Guid id) => await _context.ToDo.FindAsync(id);
    public async Task AddAsync(Todo todo) { await _context.ToDo.AddAsync(todo); await _context.SaveChangesAsync(); }
    public async Task UpdateAsync(Todo todo) { _context.Entry(todo).State = EntityState.Modified; await _context.SaveChangesAsync(); }
    public async Task DeleteAsync(Guid id) 
    { 
        var todo = await _context.ToDo.FindAsync(id);
        if (todo != null) { _context.ToDo.Remove(todo); await _context.SaveChangesAsync(); }
    }
}