using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Dtos;
using TodoApi.Migrations;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/")]
    [ApiController]
    // Inherits from ControllerBase
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/items/5
        [HttpGet("items/{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var todoItem = await _context.TodoItem.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return Ok(todoItem);
        }

        // GET: api/items/
        [HttpGet("items")]
        public async Task<ActionResult<TodoItem>> GetTodoItems()
        {
            return Ok(await _context.TodoItem.ToListAsync());
        }


        // POST: api/todolists/{listId}/items
        [HttpPost("todolists/{listId}/items")]
        public async Task<ActionResult<TodoItem>> CreateTodoItem(long listId, CreateTodoItem payload)
        {
            // load the parent TodoList 
            var todoList = await _context.TodoList.FindAsync(listId);
            if (todoList == null)
            {
                // return 404
                return NotFound($"No TodoList found with id {listId}");
            }

            // Create a new TodoItem object from the payload
            var todoItem = new TodoItem
            {
                Description = payload.Description,
                IsComplete = false, // because new items should not be completed by default
                TodoListId = listId
            };

            // Start tracking the new object
            _context.TodoItem.Add(todoItem);
            // Commit changes to the DB
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.Id }, todoItem);
        }
    }
}