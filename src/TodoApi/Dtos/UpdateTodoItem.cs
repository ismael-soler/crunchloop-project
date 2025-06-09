namespace TodoApi.Dtos;

public class UpdateTodoItem
{
    public required string Description { get; set; }
    public bool IsComplete { get; set; }
}