using Npgsql;
using TestVit.Models;

namespace TestVit.DataAccess
{
    public class CommentDataAccess
    {
        private readonly string _cs;

        public CommentDataAccess(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("Default");
        }

        public async Task<Guid> CreateCommentAsync(Comment c)
        {
            const string sql = @"
                INSERT INTO comments (id, post_id, author_id, text, created_at)
                VALUES (@id, @p, @a, @t, NOW())
                RETURNING id;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", c.Id);
            cmd.Parameters.AddWithValue("p", c.PostId);
            cmd.Parameters.AddWithValue("a", c.AuthorId);
            cmd.Parameters.AddWithValue("t", c.Text);

            return (Guid)await cmd.ExecuteScalarAsync();
        }
    }
}
