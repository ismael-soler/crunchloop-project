using System.Text.Json.Serialization;
using NuGet.Protocol.Plugins;
using TodoApi.Models;

namespace TodoApi.Models;

public class TodoItem
{
    public long Id { get; set; }
    public required string Description { get; set; }
    public bool IsComplete { get; set; }

    // Foreign key to TodoList
    public long TodoListId { get; set; }

    // Navigation property to the paren TodoList
    // Entity Framework uses this to understand the relationship between two tables. 
    // Alows to easily access the parent list from an item (e.g. myItem.TodoList)
    [JsonIgnore]
    public TodoList? TodoList { get; set; }
}