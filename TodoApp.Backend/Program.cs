using Microsoft.EntityFrameworkCore;
using TodoApp.Backend.Application.Interfaces;
using TodoApp.Backend.Infrasturcture.Persistance;
using TodoApp.Backend.Application.Handlers.Queries;
using TodoApp.Backend.Application.Handlers.Commands;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IToDoRepository, ToDoRepository>();

builder.Services.AddScoped<GetAllTodosQueryHandler>();
builder.Services.AddScoped<CreateTodoCommandHandler>();

builder.Services.AddScoped<UpdateTodoCommandHandler>();
builder.Services.AddScoped<DeleteTodoCommandHandler>();
builder.Services.AddScoped<GetTodoByIdQueryHandler>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers(); 
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()); 
app.MapControllers();   

app.Run();