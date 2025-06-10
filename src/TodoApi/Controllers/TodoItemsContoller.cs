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


        // GET: api/items/
        [HttpGet("items")]
        public async Task<ActionResult<TodoItem>> GetTodoItems()
        {
            return Ok(await _context.TodoItem.ToListAsync());
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


        // GET: api/todolists/1/items
        [HttpGet("todolists/{listId}/items")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetItemsForList(long listId)
        {
            // Check if the parent list exists.
            var todoList = await _context.TodoList.FindAsync(listId);
            if (todoList == null)
            {
                return NotFound($"No TodoList found with id {listId}");
            }

            // If the list exists, find all items that have a matching TodoListId.
            var todoItems = await _context.TodoItem
                .Where(item => item.TodoListId == listId)
                .ToListAsync();

            return Ok(todoItems);
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


        // ----------------------

        // PATCH: api/items/5/complete
        [HttpPatch("items/{id}/complete")]
        public async Task<IActionResult> CompleteTodoItem(long id)
        {
            // load item
            var todoItem = await _context.TodoItem.FindAsync(id);

            // if 404
            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.IsComplete = true;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/items/5
        [HttpDelete("items/{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            // Find the item
            var todoItem = await _context.TodoItem.FindAsync(id);

            // If not found
            if (todoItem == null)
            {
                return NotFound();
            }

            // If found, remove it and save
            _context.TodoItem.Remove(todoItem);
            await _context.SaveChangesAsync();

            // Return success
            return NoContent();
        }

        // PATCH: api/items/5/description
        [HttpPatch("items/{id}/description")]
        public async Task<IActionResult> UpdateItemDescription(long id, [FromBody] UpdateItemDescription payload)
        {
            var todoItem = await _context.TodoItem.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            todoItem.Description = payload.Description;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}