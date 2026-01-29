using TodoApp.Backend.Entities.Entities;

namespace TodoApp.Backend.Entities.Interfaces;

public interface IToDoRepository
{
    Task<IEnumerable<Todo>> GetAllAsync(Guid userId);
    Task<Todo?> GetByIdAsync(Guid id);
    Task AddAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Guid id);
    Task<User> GetOrCreateUserAsync(string email);
}