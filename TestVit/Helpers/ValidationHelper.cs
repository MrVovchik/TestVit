using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestVit.DataAccess;

namespace TestVit.Helpers
{
    public static class ValidationHelper
    {
        public static async Task<Guid?> ValidateAuth(ControllerBase controller, SessionDataAccess _session)
        {
            if (!controller.Request.Headers.TryGetValue("Authorization", out var authHeader))
                return null;

            var tokenStr = authHeader.ToString().Replace("Bearer ", "").Trim();

            if (!Guid.TryParse(tokenStr, out var tokenGuid))
                return null;

            return await _session.ValidateTokenAsync(tokenGuid);
        }
    }
}
