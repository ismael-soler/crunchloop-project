using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

// TodoContext inherits from DbContext
public class TodoContext : DbContext
{
    // NOTE: Allows the db connection details to be configured from outside the class
    // Don't really know how it works yet.
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options) { }

    // Defines the one-to-many relationship and the cascade deletion of items
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TodoList>()
            .HasMany(list => list.Items)
            .WithOne(item => item.TodoList!)
            .HasForeignKey(item => item.TodoListId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    // "I want a table in my database that will store TodoLists"
    public DbSet<TodoList> TodoList { get; set; } = default!;
    // "I want a table in my database that will store TodoItems"
    public DbSet<TodoItem> TodoItem { get; set; } = default!;
}
