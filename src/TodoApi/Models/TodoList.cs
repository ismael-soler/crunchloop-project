namespace TodoApi.Models;

public class TodoList
{
    public long Id { get; set; }
    public required string Name { get; set; }

    // To make it aware of its items or sth like that (?)
    public List<TodoItem> Items { get; set; } = new();

}
