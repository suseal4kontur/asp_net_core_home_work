namespace API.Controllers
{
    using Todo;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ViewTodos = View.Todo;

    [Route("[controller]")]
    [ApiController]
    public class TodosController : ControllerBase
    {
        private readonly ITodoRepository todoRepository;

        public TodosController(ITodoRepository todoRepository)
        {
            this.todoRepository = todoRepository ?? throw new ArgumentNullException(nameof(todoRepository));
        }

        [HttpGet]
        public async Task<ActionResult> SearchTodoItems(
            [FromQuery] ViewTodos.TodoInfoSearchQuery query,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            var searchResult = await this.todoRepository.SearchAsync(query, token).ConfigureAwait(false);
            var todoList = new ViewTodos.TodoList { todoItems = searchResult };
            return this.Ok(todoList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetAsync(string id, CancellationToken token)
        {
            try
            {
                var todoItem = await this.todoRepository.GetAsync(id, token).ConfigureAwait(false);
                return this.Ok(todoItem);
            }
            catch (TodoNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateTodoAsync(
            [FromBody] ViewTodos.TodoBuildInfo buildInfo,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                var todoInfo = await this.todoRepository.CreateAsync(buildInfo, token).ConfigureAwait(false);
                return this.Ok(todoInfo);
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(ex.Message);
            }
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> PatchAsync(
            string id,
            [FromBody] ViewTodos.TodoPatchInfo patchInfo,
            CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                var todoItem = await this.todoRepository.PatchAsync(id, patchInfo, token).ConfigureAwait(false);
                return this.Ok(todoItem);
            }
            catch (ValidationException ex)
            {
                return this.BadRequest(ex.Message);
            }
            catch (TodoNotFoundException)
            {
                return this.NotFound();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            try
            {
                await this.todoRepository.RemoveAsync(id, token).ConfigureAwait(false);
                return this.Ok();
            }
            catch (TodoNotFoundException)
            {
                return this.NotFound();
            }
        }
    }
}
