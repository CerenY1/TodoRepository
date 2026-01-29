namespace TodoApp.Backend.Entities.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public ICollection<Todo> Todos { get; set; } 
}