using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ExamBlazor.ApiRequest.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RegisterRequest
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Имя обязательно")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Роль обязательна")]
        public string Role { get; set; }
    }

    public class LoginRequest
    {
        [Required(ErrorMessage = "Email обязателен")]
        [EmailAddress(ErrorMessage = "Введите корректный email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Пароль обязателен")]
        public string Password { get; set; }
    }

    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("user")]
        public User User { get; set; }
    }

    public class UpdateUserRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Password { get; set; }
    }
} 