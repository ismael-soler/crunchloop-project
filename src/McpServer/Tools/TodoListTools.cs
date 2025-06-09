using ModelContextProtocol.Server;
using System.ComponentModel;
using System.Text.Json;

namespace McpServer.tools;

// Declare this as an MCP tool
[McpServerToolType]
public static class TodoListMcpTools
{
    [McpServerTool, Description("get Todo LISTS from the DB")]
    public static async Task<string> GetTodoLists(HttpClient client)
    // This is the http client we built on Program.cs
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
}