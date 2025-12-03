using BCrypt.Net;
using TestVit.Models;
using TestVit.DataAccess;

namespace TestVit.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserDataAccess _users;
        private readonly SessionDataAccess _sessions;

        public AuthService(UserDataAccess u, SessionDataAccess s)
        {
            _users = u;
            _sessions = s;
        }

        public async Task<Guid> RegisterAsync(RegisterRequest req)
        {
            var existing = await _users.GetByEmailAsync(req.Email);
            if (existing != null)
                throw new Exception("User already exists");

            var hash = BCrypt.Net.BCrypt.HashPassword(req.Password);

            return await _users.CreateAsync(req.Email, hash, req.DisplayName);
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest req)
        {
            var user = await _users.GetByEmailAsync(req.Email);
            if (user == null) throw new Exception("Invalid credentials");

            if (!BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
                throw new Exception("Invalid credentials");

            var (token, expiresAt) = await _sessions.CreateAsync(user.Id);

            return new AuthResponse
            {
                Token = token,
                ExpiresAt = expiresAt
            };
        }
    }
}
