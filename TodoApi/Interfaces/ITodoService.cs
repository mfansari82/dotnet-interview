using TodoApi.Models;
using TodoApi.Models.Request;

namespace TodoApi.Interfaces
{
    public interface ITodoService
    {
        Task<Todo> CreateTodo(CreateTodoRequest todo);

        Task<Todo?> GetTodoById(int id);

        Task<List<Todo>> GetAllTodos();

        Task<Todo?> UpdateTodo(int id, UpdateTodoRequest todo);

        Task<bool> DeleteTodo(int id);
    }
}
