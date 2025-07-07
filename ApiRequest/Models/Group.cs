using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExamBlazor.ApiRequest.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TeacherId { get; set; }
        public User Teacher { get; set; }
        public List<User> Students { get; set; } = new();
        public List<Lesson> Lessons { get; set; } = new();
        public int EducationalProgramId { get; set; }
        public EducationalProgram EducationalProgram { get; set; }
    }

    public class Attendance
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public bool IsPresent { get; set; }
        public DateTime Date { get; set; }
    }

    public class StudentProgress
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public User Student { get; set; }
        public int LessonId { get; set; }
        public Lesson Lesson { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CompletedAt { get; set; }
    }

    public class CreateGroupRequest
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int EducationalProgramId { get; set; }
    }

    public class AddStudentToGroupRequest
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int GroupId { get; set; }
    }

    public class AttendanceRequest
    {
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int LessonId { get; set; }
        [Required]
        public bool IsPresent { get; set; }
    }
} 