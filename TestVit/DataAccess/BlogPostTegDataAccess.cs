using Npgsql;
using TestVit.Models;

namespace TestVit.DataAccess
{
    public class BlogPostTegDataAccess
    {
        private readonly string _cs;

        public BlogPostTegDataAccess(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("Default");
        }

        public async Task<List<BlogPostTeg>> GetAllPostTagsAsync()
        {
            const string sql = @"
            SELECT post_id, tag_id
            FROM blog_post_tags;
        ";

            var list = new List<BlogPostTeg>();

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();
            await using var cmd = new NpgsqlCommand(sql, conn);

            using var r = await cmd.ExecuteReaderAsync();

            while (await r.ReadAsync())
            {
                list.Add(new BlogPostTeg
                {
                    PostId = r.GetGuid(0),
                    TagId = r.GetGuid(1)
                });
            }

            return list;
        }
    }
}
