using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace McpServer.tools;

// Declare this as an MCP tool
[McpServerToolType]
public static class TodoListMcpTools
{
    [McpServerTool, Description("get Todo LISTS from the Database")]
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

    [McpServerTool, Description("create a Todo List in the Database")]
    public static async Task<string> PostTodoList(
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
}