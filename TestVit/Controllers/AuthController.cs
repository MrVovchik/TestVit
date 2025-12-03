using Microsoft.AspNetCore.Mvc;
using TestVit.Models;
using TestVit.Services;

namespace TestVit.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _auth;

        public AuthController(IAuthService auth) => _auth = auth;

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest req)
        {
            var id = await _auth.RegisterAsync(req);
            return Ok(new { userId = id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            try
            {
                var resp = await _auth.LoginAsync(req);
                return Ok(resp);
            }
            catch
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }
        }
    }
}
