using Npgsql;
using TestVit.Models;

namespace TestVit.DataAccess
{
    public class BlogPostDataAccess
    {
        private readonly string _cs;

        public BlogPostDataAccess(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("Default");
        }

        public async Task<List<BlogPost>> GetAllAsync()
        {
            const string sql = @"
            SELECT id, author_id, title, body, created_at
            FROM blog_posts;";

            var list = new List<BlogPost>();

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);

            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
            {
                list.Add(new BlogPost
                {
                    Id = r.GetGuid(0),
                    AuthorId = r.GetGuid(1),
                    Title = r.GetString(2),
                    Body = r.GetString(3),
                    CreatedAt = r.GetDateTime(4)
                });
            }

            return list;
        }

        public async Task<Guid> CreatePostAsync(Guid authorId, string title, string body)
        {
            const string sql = @"
                INSERT INTO blog_posts (id, author_id, title, body, created_at)
                VALUES (@id, @a, @t, @b, NOW())
                RETURNING id;";

            var id = Guid.NewGuid();

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("a", authorId);
            cmd.Parameters.AddWithValue("t", title);
            cmd.Parameters.AddWithValue("b", body);

            return (Guid)await cmd.ExecuteScalarAsync();
        }

        public async Task AddTagToPostAsync(Guid postId, Guid tagId)
        {
            const string sql = @"
                INSERT INTO blog_post_tags (post_id, tag_id)
                VALUES (@p, @t)
                ON CONFLICT DO NOTHING;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("p", postId);
            cmd.Parameters.AddWithValue("t", tagId);

            await cmd.ExecuteNonQueryAsync();
        }

        public async Task<List<BlogPost>> GetPostsByTagsAsync(IEnumerable<Guid> tagIds)
        {
            var ids = tagIds.ToList();
            if (ids.Count == 0) return new();

            // находим посты, у которых есть теги из запроса
            const string sql = @"
                SELECT p.id, p.author_id, p.title, p.body, p.created_at
                FROM blog_posts AS p
                JOIN blog_post_tags AS pt ON pt.post_id = p.id
                WHERE pt.tag_id = ANY(@tagIds)
                ORDER BY p.created_at DESC;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("tagIds", ids);
            cmd.Parameters.AddWithValue("tagCount", ids.Count);

            var posts = new List<BlogPost>();

            using var r = await cmd.ExecuteReaderAsync();
            while (await r.ReadAsync())
            {
                posts.Add(new BlogPost
                {
                    Id = r.GetGuid(0),
                    AuthorId = r.GetGuid(1),
                    Title = r.GetString(2),
                    Body = r.GetString(3),
                    CreatedAt = r.GetDateTime(4),
                });
            }

            return posts;
        }

        public async Task<BlogPost?> GetPostByIdAsync(Guid postId)
        {
            const string sql = @"
                SELECT id, author_id, title, body, created_at
                FROM blog_posts
                WHERE id = @id
                LIMIT 1;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", postId);

            using var r = await cmd.ExecuteReaderAsync();

            if (!await r.ReadAsync())
                return null;

            return new BlogPost
            {
                Id = r.GetGuid(0),
                AuthorId = r.GetGuid(1),
                Title = r.GetString(2),
                Body = r.GetString(3),
                CreatedAt = r.GetDateTime(4),
            };
        }
    }
}
