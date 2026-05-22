using Microsoft.Data.Sqlite;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Models.Request;

namespace TodoApi.Services
{
    public class TodoService : ITodoService
    {
        private readonly string _connectionString;
        private readonly ILogger<TodoService> _logger;

        public TodoService(IConfiguration configuration, ILogger<TodoService> logger)
        {
            _connectionString = configuration["ConnectionStrings:DefaultConnection"];
            _logger = logger;
        }

        public async Task<Todo> CreateTodo(CreateTodoRequest todo)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var createdAt = DateTime.UtcNow;

            var command = connection.CreateCommand();
            command.CommandText = $@"
                INSERT INTO Todos (Title, Description, IsCompleted, CreatedAt)
                VALUES ('{todo.Title}', '{todo.Description}', {(todo.IsCompleted ? 1 : 0)}, '{createdAt}');
                SELECT last_insert_rowid();
            ";

            var id = Convert.ToInt32(await command.ExecuteScalarAsync());

            _logger.LogInformation("Todo created with Id {Id}", id);

            return new Todo
            {
                Id = id,
                Title = todo.Title,
                Description = todo.Description,
                IsCompleted = todo.IsCompleted,
                CreatedAt = createdAt
            };
        }

        public async Task<List<Todo>> GetAllTodos()
        {
            var todos = new List<Todo>();
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Todos";

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                todos.Add(new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                });
            }

            _logger.LogInformation("Fetched {Count} todos", todos.Count);

            return todos;
        }

        public async Task<Todo?> GetTodoById(int id)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Todos WHERE Id = {id}";

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return new Todo
                {
                    Id = reader.GetInt32(0),
                    Title = reader.GetString(1),
                    Description = reader.GetString(2),
                    IsCompleted = reader.GetInt32(3) == 1,
                    CreatedAt = DateTime.Parse(reader.GetString(4))
                };
            }

            return null;
        }

        public async Task<Todo?> UpdateTodo(int id, UpdateTodoRequest todo)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $@"
                UPDATE Todos
                SET Title = '{todo.Title}', Description = '{todo.Description}', IsCompleted = {(todo.IsCompleted ? 1 : 0)}
                WHERE Id = {id}
            ";

            var rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected == 0)
            {
                _logger.LogWarning("Todo not found for update. Id {Id}", id);
                return null;
            }

            _logger.LogInformation("Todo updated with Id {Id}", id);

            return await GetTodoById(id);
        }

        public async Task<bool> DeleteTodo(int id)
        {
            await using var connection = new SqliteConnection(_connectionString);
            await connection.OpenAsync();

            var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Todos WHERE Id = {id}";

            var rowsAffected = await command.ExecuteNonQueryAsync();

            if (rowsAffected > 0)
            {
                _logger.LogInformation("Todo deleted with Id {Id}", id);

                return true;
            }

            _logger.LogWarning("Todo not found for delete. Id {Id}", id);

            return false;
        }
    }
}
