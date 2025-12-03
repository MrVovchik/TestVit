using Npgsql;
using TestVit.Models;

namespace TestVit.DataAccess
{
    public class UserDataAccess
    {
        private readonly string _cs;

        public UserDataAccess(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("Default");
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            const string sql = "SELECT id, email, password_hash, display_name FROM users WHERE email = @e;";
            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("e", email);

            using var r = await cmd.ExecuteReaderAsync();
            if (!await r.ReadAsync()) return null;

            return new User
            {
                Id = r.GetGuid(0),
                Email = r.GetString(1),
                PasswordHash = r.GetString(2),
                DisplayName = r.GetString(3)
            };
        }

        public async Task<Guid> CreateAsync(string email, string passwordHash, string displayName)
        {
            const string sql = @"
                INSERT INTO users (email, password_hash, display_name) 
                VALUES (@e, @ph, @dn)
                RETURNING id;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("e", email);
            cmd.Parameters.AddWithValue("ph", passwordHash);
            cmd.Parameters.AddWithValue("dn", (object?)displayName ?? DBNull.Value);

            return (Guid)await cmd.ExecuteScalarAsync();
        }
    }
}
