namespace TestVit.Models
{
    public class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string DisplayName { get; set; } // имя в системе
    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class AuthResponse
    {
        public Guid Token { get; set; } // тут может быть jwt token, для упрощения id in guid type
        public DateTime ExpiresAt { get; set; }
    }
}
