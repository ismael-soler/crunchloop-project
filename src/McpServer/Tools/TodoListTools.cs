using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Net.Mime;
using System.Text.Json;

namespace McpServer.tools;

// Declare this as an MCP tool
[McpServerToolType]
public static class TodoListMcpTools
{
    [McpServerTool, Description("get ALL To-do Lists from the Database")]
    // Uses the http client we built on Program.cs
    public static async Task<string> GetTodoLists(HttpClient client)
    {
        // call the api
        using var JsonDocument = await client.ReadJsonDocumentAsync("/api/todolists");
        // returns a JsonElement
        var root = JsonDocument.RootElement;

        if (root.GetArrayLength() == 0)
        {
            return "No lists available.";
        }

        return root.GetRawText();
    }

    [McpServerTool, Description("get ONE To-do List from the Database using it's Id")]
    public static async Task<string> GetTodoListsById(
        HttpClient client,
        [Description("The ID of the to-do list")] long id)
    {
        try
        {
            // call the api
            using var JsonDocument = await client.ReadJsonDocumentAsync($"/api/todolists/{id}");
            // The API returns one object so we just return its text.
            return JsonDocument.RootElement.GetRawText();
        }
        catch (HttpRequestException ex)
        {
            // if not found
            if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return $"No to-do list for id {id}.";
            }

            // for any other possible errors
            return $"An API error occurred: {ex.StatusCode}";
        }
    }

    [McpServerTool, Description("create ONLY ONE Todo List in the Database")]
    public static async Task<string> CreateTodoList(
        HttpClient client,
        [Description("The name of the new to-do list to create")] string name)
    {
        // Create an objet representing the JSON payload
        var payload = new { Name = name };

        // serialize the object
        var jsonPayload = JsonSerializer.Serialize(payload);

        // Create the object that the HttpClient uses to represent the body of the request.
        var content = new StringContent(
            jsonPayload,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // send the POST request
        var response = await client.PostAsync("/api/todolists", content);

        // check the response and return a message
        if (response.IsSuccessStatusCode)
        {
            return $"Successfully created the to-do list named '{name}'.";
        }
        else
        {
            return $"Error creating list. The API responded with: {response.StatusCode}";
        }
    }

    [McpServerTool, Description("Update ONLY ONE Todo List name using it's id")]
    public static async Task<string> UpdateTodoList(
        HttpClient client,
        [Description("The ID of the LIST to be updated")] long id,
        [Description("The new name of the List")] string name
    )
    {
        // Create an object representing the json payload
        var payload = new { Name = name };

        // serialize the object
        var jsonPayload = JsonSerializer.Serialize(payload);

        // Create the body object
        var content = new StringContent(
            jsonPayload,
            System.Text.Encoding.UTF8,
            "application/json"
        );

        // Send the PUT request
        var response = await client.PutAsync($"api/todolists/{id}", content);

        // check the response and return a message 
        if (response.IsSuccessStatusCode)
        {
            return $"Succesffully updated the to-do list of id {id} to {name}";
        }
        else
        {
            return $"Error updating the list. The API responded with {response.StatusCode}";
        }
    }

    [McpServerTool, Description("Delete ONLY ONE Todo List using it's ID")]
    public static async Task<string> DeleteTodoList(
        HttpClient client,
        [Description("The ID of the LIST to be DELETED")] long id
        )
    {
        // send the DELETE request
        var response = await client.DeleteAsync($"/api/todolists/{id}");

        // check the response and return a message 
        if (response.IsSuccessStatusCode)
        {
            return $"Succesffully deleted the to-do list of id {id}";
        }
        else
        {
            return $"Error deleting the list. The API responded with {response.StatusCode}";
        }
    }


}