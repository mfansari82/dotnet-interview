using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Models.Request;
using TodoApi.Services;

namespace TodoApi.Tests
{
    public class TodoServiceTests
    {
        private readonly TodoService _service;
        private readonly IConfiguration _connectionString;

        public TodoServiceTests()
        {
            var configurationMock = new Mock<IConfiguration>();

            configurationMock.Setup(x => x["ConnectionStrings:DefaultConnection"]).Returns("Data Source=test_todos.db");

            var loggerMock = new Mock<ILogger<TodoService>>();

            _service = new TodoService(configurationMock.Object, loggerMock.Object);

            InitializeDatabase();
        }

        [Fact]
        public async Task CreateTodo_ShouldCreateTodo()
        {
            var request = new CreateTodoRequest
            {
                Title = "Test Todo",
                Description = "Test Description",
                IsCompleted = false
            };

            var result = await _service.CreateTodo(request);

            Assert.NotNull(result);

            Assert.True(result.Id > 0);

            Assert.Equal(request.Title, result.Title);
        }

        [Fact]
        public async Task GetAllTodos_ShouldReturnTodos()
        {
            var result = await _service.GetAllTodos();
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTodoById_ShouldReturnTodo_WhenExists()
        {
            var createdTodo = await _service.CreateTodo(
                    new CreateTodoRequest
                    {
                        Title = "GetTodoById",
                        Description = "Test",
                        IsCompleted = false
                    });

            var result = await _service.GetTodoById(createdTodo.Id);

            Assert.NotNull(result);

            Assert.Equal(createdTodo.Id, result.Id);
        }

        [Fact]
        public async Task GetTodoById_ShouldReturnNull_WhenNotExists()
        {
            var result = await _service.GetTodoById(5);

            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateTodo_ShouldUpdateTodo_WhenExists()
        {
            var createdTodo = await _service.CreateTodo(
                    new CreateTodoRequest
                    {
                        Title = "OldTodo",
                        Description = "Old Todo Desc",
                        IsCompleted = false
                    });

            var updateRequest = new UpdateTodoRequest
            {
                Title = "UpdatedTodo",
                Description = "Updated Todo Desc",
                IsCompleted = true
            };

            var result = await _service.UpdateTodo(createdTodo.Id, updateRequest);

            Assert.NotNull(result);

            Assert.Equal("UpdatedTodo", result.Title);

            Assert.True(result.IsCompleted);
        }

        [Fact]
        public async Task UpdateTodo_ShouldReturnNull_WhenTodoMissing()
        {
            var updateRequest = new UpdateTodoRequest
            {
                Title = "UpdatedTodo",
                Description = "Updated Todo Desc",
                IsCompleted = true
            };

            var result = await _service.UpdateTodo(5, updateRequest);

            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteTodo_ShouldReturnTrue_WhenDeleted()
        {
            var createdTodo =
                await _service.CreateTodo(
                    new CreateTodoRequest
                    {
                        Title = "DeleteTodo",
                        Description = "Delete Todo Desc",
                        IsCompleted = false
                    });

            var result = await _service.DeleteTodo(createdTodo.Id);

            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTodo_ShouldReturnFalse_WhenNotFound()
        {
            var result = await _service.DeleteTodo(1234);

            Assert.False(result);
        }

        private void InitializeDatabase()
        {
            using var connection =
                new SqliteConnection(
                    "Data Source=test_todos.db");

            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = @"
            CREATE TABLE IF NOT EXISTS Todos (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Title TEXT NOT NULL,
            Description TEXT,
            IsCompleted INTEGER NOT NULL DEFAULT 0,
            CreatedAt TEXT NOT NULL
            )";

            command.ExecuteNonQuery();
        }
    }
}