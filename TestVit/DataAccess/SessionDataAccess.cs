using Npgsql;

namespace TestVit.DataAccess
{
    public class SessionDataAccess
    {
        private readonly string _cs;
        public SessionDataAccess(IConfiguration cfg) => _cs = cfg.GetConnectionString("Default");

        public async Task<(Guid Token, DateTime ExpiresAt)> CreateAsync(Guid userId)
        {
            const string sql = @"
                INSERT INTO sessions (user_id, expires_at)
                VALUES (@uid, @exp)
                RETURNING token, expires_at;";

            var expiresAt = DateTime.UtcNow.AddDays(7);

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("uid", userId);
            cmd.Parameters.AddWithValue("exp", expiresAt);

            using var r = await cmd.ExecuteReaderAsync();
            await r.ReadAsync();

            return (r.GetGuid(0), r.GetDateTime(1));
        }

        public async Task<Guid?> ValidateTokenAsync(Guid token)
        {
            const string sql = @"
                SELECT user_id FROM sessions
                WHERE token = @t AND expires_at > now();";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("t", token);

            var obj = await cmd.ExecuteScalarAsync();
            return obj == null ? null : (Guid?)obj;
        }
    }
}
