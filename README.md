# TodoList MCP Challenge

This project provides a complete environment for a to-do list application. It features a .NET REST API for managing tasks and lists, which is then exposed to a large language model client via a Model Context Protocol (MCP) server. The entire application is containerized with Docker for easy setup and deployment.

---

### Tech Stack

-   **Backend API:** C#, ASP.NET Core
-   **Database:** MS SQL Server, Entity Framework Core
-   **MCP Server:** .NET, ModelContextProtocol Library
-   **Containerization:** Docker, Docker Compose

---

### Features
- **RESTful API**: A fully functional API to create, read, update, and delete to-do lists and items.
- **MCP Server**: Exposes API functionality as tools for a language model (e.g., Claude).
- **Fully Containerized**: All services (API, database, MCP server) are managed by Docker Compose.
- **Database Migrations**: Entity Framework Core migrations are automatically applied on startup.
- **Relationship-Aware Deletion**: Deleting a to-do list automatically cascades to delete all its associated items.

---

### Project Structure
The project has two main services within the `src` directory:
- `src/TodoApi/`: ASP.NET Core API. It handles all business logic, databases interactions, and exposes the HTTP endpoints.
- `src/McpServer/`: The .NET console app that runs the MCP Server. It communicates with the API service to execute commands received from the client via standard input/output.

---

### Prerequisites
Before you begin, make sure you have the following installed on your system:
- [Docker](https://docs.docker.com/get-started/get-docker/)
- [Docker Compose](https://docs.docker.com/compose/install/)

---

### How to Run the Project

**1. Navigate to the Project Directory**

Open you terminal and navigate to the root of this project.

**2. Build and Run the Application**

Execute the following command to build the Docker images and start all services in detached mode:

```bash
docker-compose up --build -d
```
- It will start three services defined in `docker-compose.yml`: `sqlserver`, `api`, and `mcpserver`.
- The api service includes an entrypoint script that waits for the database to be ready before applying migrations and starting the server.

**3. Confirm the API is running correctly (Optional, but important)**

To confirm the API is up and running:
```bash
docker-compose logs -f api
```

The applicatoin is fully functional once you see the following message in your terminal:
`Now listening on: http://[::]:8080`. 

Once you see this message, you can press `Ctrl + C` to stop viewing the logs. The application services will continue to run in the background. 

**4. Stopping the Application**
To stop all the running containers, use the following command:
``` bash
docker-compose down
```

---

### How to Set Up Claude Desktop

To connect to the MCP server through Claude Desktop client, follow these steps:
1. Open Claude Desktop Settings and navigate to the **Developer** tab.
2. CLick on "Edit Config", this will open a folder that contains the `claude_desktop_config.json` file. Open it with any text editor.
3. Paste the following content so the file looks like this:
```JSON
{
    "mcpServers": {
        "todo_mcp_docker": {
        "command": "docker",
        "args": [
            "exec",
            "-i",
            "crunchloop-mcpserver-1",
            "dotnet",
            "McpServer.dll"
            ]
        }
    }
}
```
4. Save the file and restart Claude Desktop, it will now have access to the to-do list tools.
---

### Available MCP Tools
The MCP server exposes the following tools, which are defined from the `TodoItemMcpTools` and `TodoListMcpTools` classes.

### To-do List Tools
- `GetTodoLists`: Gets all lists
- `GetTodoListsById`: Gets a specific list using its ID
- `CreateTodoList`: Creates a new Todo list with a given name
- `UpdateTodoList`: Updates the name of an existing Todo list using its ID
- `DeleteTodoList`: Deletes a to-do list an dll of its items by its ID

### To-do Item Tools:
- `GetItems`: Gets all items
- `CreateItem`: Creates a new task with a description and links it to an existing to-do list by its ID
- `GetItemsFromList`: Gets all items froma specific to-do list by its ID
- `CompleteTodoItem`: Marks a specific item as complete
- `DeleteTodoItem`: Deletes a specific task
- `UpdateItemDescription`: Updates the description of a specific task.

---


### Video Demo

[![IMAGE ALT TEXT](http://img.youtube.com/vi/Um32bAb4Ml4/0.jpg)](http://www.youtube.com/watch?v=Um32bAb4Ml4)

---
