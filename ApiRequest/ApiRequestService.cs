using Microsoft.JSInterop;
using ExamBlazor.ApiRequest.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ExamBlazor.ApiRequest
{
    public class ApiRequestService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _runtime;
        private User _currentUser;

        public ApiRequestService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _runtime = jsRuntime;
        }

        // Аутентификация
        public async Task<bool> Login(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/User/Login", request);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (loginResponse?.Token != null)
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.Token);
                    _currentUser = loginResponse.User;
                    await _runtime.InvokeVoidAsync("localStorage.setItem", "token", loginResponse.Token);
                    await _runtime.InvokeVoidAsync("localStorage.setItem", "user", JsonSerializer.Serialize(_currentUser));
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Register(RegisterRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/User/Register", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task Logout()
        {
            _currentUser = null;
            _httpClient.DefaultRequestHeaders.Authorization = null;
            await _runtime.InvokeVoidAsync("localStorage.removeItem", "token");
            await _runtime.InvokeVoidAsync("localStorage.removeItem", "user");
        }

        public async Task<User> GetCurrentUser()
        {
            if (_currentUser != null) return _currentUser;

            try
            {
                var token = await _runtime.InvokeAsync<string>("localStorage.getItem", "token");
                if (!string.IsNullOrEmpty(token))
                {
                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    var userJson = await _runtime.InvokeAsync<string>("localStorage.getItem", "user");
                    if (!string.IsNullOrEmpty(userJson))
                    {
                        _currentUser = JsonSerializer.Deserialize<User>(userJson, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return _currentUser;
                    }
                }
            }
            catch { }
            return null;
        }

        // Пользователи
        public async Task<List<User>> GetAllUsers()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/User/GetAllUsers");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<User>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<User>();
            }
            catch
            {
                return new List<User>();
            }
        }

        // Образовательные программы
        public async Task<List<EducationalProgram>> GetEducationalPrograms()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/EducationalProgram");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<EducationalProgram>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<EducationalProgram>();
            }
            catch
            {
                return new List<EducationalProgram>();
            }
        }

        public async Task<bool> CreateEducationalProgram(EducationalProgram program)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/EducationalProgram", program);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Группы
        public async Task<List<Group>> GetGroups()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Group");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Group>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Group>();
            }
            catch
            {
                return new List<Group>();
            }
        }

        public async Task<bool> CreateGroup(CreateGroupRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Group", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddStudentToGroup(AddStudentToGroupRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Group/AddStudent", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Уроки
        public async Task<List<Lesson>> GetLessons()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Lesson");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Lesson>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Lesson>();
            }
            catch
            {
                return new List<Lesson>();
            }
        }

        // Материалы
        public async Task<List<Material>> GetMaterials()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Material");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Material>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Material>();
            }
            catch
            {
                return new List<Material>();
            }
        }

        // Сообщения
        public async Task<List<Message>> GetMessages(int lessonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Message/Lesson/{lessonId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Message>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Message>();
            }
            catch
            {
                return new List<Message>();
            }
        }

        public async Task<bool> CreateMessage(CreateMessageDto message)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Message", message);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        // Посещаемость
        public async Task<bool> MarkAttendance(AttendanceRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Attendance", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Attendance>> GetAttendance(int lessonId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/Attendance/Lesson/{lessonId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Attendance>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<Attendance>();
            }
            catch
            {
                return new List<Attendance>();
            }
        }

        // Прогресс
        public async Task<List<StudentProgress>> GetStudentProgress(int studentId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/StudentProgress/Student/{studentId}");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<StudentProgress>>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }) ?? new List<StudentProgress>();
            }
            catch
            {
                return new List<StudentProgress>();
            }
        }
    }
} 