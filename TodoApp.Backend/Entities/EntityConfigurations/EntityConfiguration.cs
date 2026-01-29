using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TodoApp.Backend.Entities.Entities;

namespace TodoApp.Backend.Entities.EntityConfigurations;

public class TodoConfiguration : IEntityTypeConfiguration<Todo>
{
    public void Configure(EntityTypeBuilder<Todo> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Title).IsRequired().HasMaxLength(100);
        builder.Property(t => t.IsCompleted).HasDefaultValue(false);
        
        builder.HasOne(t => t.User)
               .WithMany(u => u.Todos)
               .HasForeignKey(t => t.UserId);
    }
}