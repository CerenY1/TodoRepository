using Microsoft.EntityFrameworkCore;
using TodoApp.Backend.Domain;

namespace TodoApp.Backend.Infrasturcture.Persistance;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Todo> ToDo { get; set; }
}