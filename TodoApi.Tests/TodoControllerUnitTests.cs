using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TodoApi.Controllers;
using TodoApi.Interfaces;
using TodoApi.Models;
using TodoApi.Models.Request;

namespace TodoApi.Tests
{
    public class TodoControllerUnitTests
    {
        private readonly Mock<ITodoService> _todoServiceMock;
        private readonly Mock<ILogger<TodoController>> _loggerMock;
        private readonly TodoController _controller;

        public TodoControllerUnitTests()
        {
            _todoServiceMock = new Mock<ITodoService>();

            _loggerMock = new Mock<ILogger<TodoController>>();

            _controller = new TodoController(_todoServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task CreateTodo_ReturnsOkResult()
        {
            var request = new CreateTodoRequest
            {
                Title = "Test Todo",
                Description = "Test Desc",
                IsCompleted = false
            };

            var response = new Todo
            {
                Id = 1,
                Title = request.Title,
                Description = request.Description,
                IsCompleted = request.IsCompleted,
                CreatedAt = DateTime.UtcNow
            };

            _todoServiceMock.Setup(x => x.CreateTodo(request)).ReturnsAsync(response);

            var result = await _controller.CreateTodo(request);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var todo = Assert.IsType<Todo>(okResult.Value);

            Assert.Equal(1, todo.Id);
        }

        [Fact]
        public async Task GetTodo_ReturnsNotFound_WhenTodoMissing()
        {
            _todoServiceMock.Setup(x => x.GetTodoById(1)).ReturnsAsync((Todo?)null);

            var result = await _controller.GetTodo(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetTodo_ReturnsOk_WhenTodoExists()
        {
            var todo = new Todo
            {
                Id = 1,
                Title = "Test"
            };

            _todoServiceMock.Setup(x => x.GetTodoById(1)).ReturnsAsync(todo);

            var result = await _controller.GetTodo(1);

            var okResult = Assert.IsType<OkObjectResult>(result);

            var returnedTodo = Assert.IsType<Todo>(okResult.Value);

            Assert.Equal(1, returnedTodo.Id);
        }

        [Fact]
        public async Task DeleteTodo_ReturnsNoContent()
        {
            _todoServiceMock.Setup(x => x.DeleteTodo(1)).ReturnsAsync(true);

            var result = await _controller.DeleteTodo(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTodo_ReturnsNotFound()
        {
            _todoServiceMock.Setup(x => x.DeleteTodo(1)).ReturnsAsync(false);

            var result = await _controller.DeleteTodo(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateTodo_ReturnsNotFound()
        {
            var request = new UpdateTodoRequest
            {
                Title = "Updated"
            };

            _todoServiceMock.Setup(x => x.UpdateTodo(1, request)).ReturnsAsync((Todo?)null);

            var result = await _controller.UpdateTodo(1, request);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateTodo_ReturnsOk()
        {
            var request = new UpdateTodoRequest
            {
                Title = "Updated"
            };

            var todo = new Todo
            {
                Id = 1,
                Title = "Updated"
            };

            _todoServiceMock.Setup(x => x.UpdateTodo(1, request)).ReturnsAsync(todo);

            var result = await _controller.UpdateTodo(1, request);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.NotNull(okResult.Value);
        }
    }
}