using Npgsql;
using TestVit.Models;

namespace TestVit.DataAccess
{
    public class LikeDataAccess
    {
        private readonly string _cs;

        public LikeDataAccess(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("Default");
        }

        public async Task<List<Like>> GetAllAsync()
        {
            const string sql = @"
                SELECT id, post_id, user_id, created_at
                FROM likes;";

            var list = new List<Like>();

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            using var r = await cmd.ExecuteReaderAsync();

            while (await r.ReadAsync())
            {
                list.Add(new Like
                {
                    Id = r.GetGuid(0),
                    PostId = r.GetGuid(1),
                    UserId = r.GetGuid(2),
                    CreatedAt = r.GetDateTime(3)
                });
            }

            return list;
        }

        public async Task<Like?> GetLikeByPostIdByUserIdAsync(Guid postId, Guid userId)
        {
            const string sql = @"
                SELECT id, post_id, user_id, created_at
                FROM likes
                WHERE post_id = @p AND user_id = @u
                LIMIT 1;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p", postId);
            cmd.Parameters.AddWithValue("u", userId);

            using var r = await cmd.ExecuteReaderAsync();

            if (!await r.ReadAsync())
                return null;

            return new Like
            {
                Id = r.GetGuid(0),
                PostId = r.GetGuid(1),
                UserId = r.GetGuid(2),
                CreatedAt = r.GetDateTime(3),
            };
        }

        public async Task<Guid> CreateLikeAsync(Like l)
        {
            const string sql = @"
                INSERT INTO likes (id, post_id, user_id, created_at)
                VALUES (@id, @p, @u, NOW())
                RETURNING id;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", l.Id);
            cmd.Parameters.AddWithValue("p", l.PostId);
            cmd.Parameters.AddWithValue("u", l.UserId);

            return (Guid)await cmd.ExecuteScalarAsync();
        }
    }
}
