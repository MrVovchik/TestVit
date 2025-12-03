using TestVit.Models;

namespace TestVit.Services
{
    public interface IAuthService
    {
        public Task<Guid> RegisterAsync(RegisterRequest req);

        public Task<AuthResponse> LoginAsync(LoginRequest req);
    }
}
