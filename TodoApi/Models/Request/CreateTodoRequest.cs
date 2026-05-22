using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models.Request
{
    public class CreateTodoRequest
    {
        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsCompleted { get; set; } = false;
    }
}
