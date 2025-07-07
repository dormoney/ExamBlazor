using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExamBlazor.ApiRequest.Models
{
    public class EducationalProgram
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public List<Lesson> Lessons { get; set; } = new();
    }

    public class Lesson
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int EducationalProgramId { get; set; }
        public EducationalProgram EducationalProgram { get; set; }
        public List<Material> Materials { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }

    public class Material
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
    }

    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsEdited { get; set; }
        public DateTime? EditedAt { get; set; }
    }

    public class CreateMessageDto
    {
        [Required]
        public string Content { get; set; }
        [Required]
        public int LessonId { get; set; }
    }

    public class UpdateMessageDto
    {
        [Required]
        public string Content { get; set; }
    }
} 