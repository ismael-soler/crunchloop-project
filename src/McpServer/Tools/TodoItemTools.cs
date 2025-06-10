using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net.Mime;
using System.Text.Json;

namespace McpServer.tools;

[McpServerToolType]
public static class TodoItemMcpTools
{
    [McpServerTool, Description("get ALL items/tasks regardless of their List or Completion state")]
    public static async Task<string> GetItems(HttpClient client)
    {
        // call the api
        using var JsonDocument = await client.ReadJsonDocumentAsync("/api/items");
        var root = JsonDocument.RootElement;

        if (root.GetArrayLength() == 0)
        {
            return "No items available.";
        }

        return root.GetRawText();
    }

    [McpServerTool, Description("Create ONLY ONE Item/task, and link it to an existing List")]
    public static async Task<string> CreateItem(
        HttpClient client,
        [Description("The description of the item or task to create")] string description,
        [Description("The ID of the Todo List that this item/task will be linked to")] long listId
    )
    {
        // Create an object representing the JSON payload
        var payload = new { Description = description };

        // serialize the object
        var jsonPayload = JsonSerializer.Serialize(payload);

        // Create the object that represents the body of the request
        var content = new StringContent(
            jsonPayload,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // send the POST request
        var response = await client.PostAsync($"/api/todolists/{listId}/items", content);

        // check the response and return a message
        if (response.IsSuccessStatusCode)
        {
            return $"Succesffully created the item of description {description} on the list of id {listId}";
        }
        else
        {
            return $"Error creating list. The API responded with: {response.StatusCode}";
        }
    }

    [McpServerTool, Description("Get all items/tasks from a specific To-do List by its ID")]
    public static async Task<string> GetItemsFromList(
        HttpClient client,
        [Description("The ID of the to-do List")] long listId)
    {
        try
        {
            // call the api
            using var jsonDocument = await client.ReadJsonDocumentAsync($"/api/todolists/{listId}/items");
            var root = jsonDocument.RootElement;

            // if the lists exists but is empty
            if (root.GetArrayLength() == 0)
            {
                return $"The list with ID {listId} exists, but it has no items.";
            }
            return root.GetRawText();
        }
        // if the lists does not exist
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"No to-do list for id {listId}.";
            }

            // for any other possible errors
            return $"An API error occurred: {ex.StatusCode}";
        }
    }

    [McpServerTool, Description("Marks a to-do item/task as complete using its ID to locate it")]
    public static async Task<string> CompleteTodoItem(
        HttpClient client,
        [Description("The ID of the item to mark as complete")] long itemId)
    {

        var content = new StringContent(
            "",
        System.Text.Encoding.UTF8,
        "application/json");

        var response = await client.PatchAsync($"/api/items/{itemId}/complete", content);

        if (response.IsSuccessStatusCode)
        {
            return $"Succesfully marked item {itemId} as complete.";
        }
        else
        {
            return $"Error updating status. The API responded with: {response.StatusCode}";
        }
    }


    [McpServerTool, Description("Deletes a specific to-do item/task using its ID")]
    public static async Task<string> DeleteTodoItem(
       HttpClient client,
       [Description("The ID of the item to delete")] long itemId)
    {
        // send the DELETE request
        var response = await client.DeleteAsync($"/api/items/{itemId}");

        // check the response and return a message
        if (response.IsSuccessStatusCode)
        {
            return $"Successfully deleted item {itemId}.";
        }
        return $"Error deleting item. The API responded with: {response.StatusCode}";
    }

    [McpServerTool, Description("Updates the description of a specific todo item/task using it's id")]
    public static async Task<string> UpdateItemDescription(
        HttpClient client,
        [Description("The ID of the item to update")] long itemId,
        [Description("The new description for the item")] string newDescription)
    {
        var payload = new { Description = newDescription };
        var jsonPayload = JsonSerializer.Serialize(payload);

        var content = new StringContent(
            jsonPayload,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        var response = await client.PatchAsync($"/api/items/{itemId}/description", content);

        if (response.IsSuccessStatusCode)
        {
            return $"Successfully updated the item's description";
        }
        return $"Error updating description. The API responded with: {response.StatusCode}";        
    }
}