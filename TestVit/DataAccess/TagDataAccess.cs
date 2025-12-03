using Npgsql;
using TestVit.Models;

namespace TestVit.DataAccess
{
    public class TagDataAccess
    {
        private readonly string _cs;

        public TagDataAccess(IConfiguration cfg)
        {
            _cs = cfg.GetConnectionString("Default");
        }

        public async Task<Guid> CreateTagAsync(string name)
        {
            const string sql = @"
                INSERT INTO tags (id, name)
                VALUES (@id, @name)
                RETURNING id;";

            var id = Guid.NewGuid();

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);
            cmd.Parameters.AddWithValue("name", name);

            return (Guid)await cmd.ExecuteScalarAsync();
        }

        public async Task<Tag?> GetByNameAsync(string name)
        {
            const string sql = @"SELECT id, name FROM tags WHERE name = @n;";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("n", name);

            using var r = await cmd.ExecuteReaderAsync();
            if (!await r.ReadAsync()) return null;

            return new Tag
            {
                Id = r.GetGuid(0),
                Name = r.GetString(1)
            };
        }

        public async Task<List<Tag>?> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            var list = ids.ToList();
            if (list.Count == 0) return new();

            const string sql = @"SELECT id, name FROM tags WHERE id = ANY(@id);";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", ids);

            var result = new List<Tag>();
            using var r = await cmd.ExecuteReaderAsync();

            while (await r.ReadAsync())
            {
                result.Add(new Tag
                {
                    Id = r.GetGuid(0),
                    Name = r.GetString(1)
                });
            }

            return result;
        }

        public async Task<List<Tag>> GetByNamesAsync(IEnumerable<string> names)
        {
            var list = names.ToList();
            if (list.Count == 0) return new();

            const string sql = @"SELECT id, name FROM tags WHERE name = ANY(@names);";

            await using var conn = new NpgsqlConnection(_cs);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("names", list);

            var result = new List<Tag>();
            using var r = await cmd.ExecuteReaderAsync();

            while (await r.ReadAsync())
            {
                result.Add(new Tag
                {
                    Id = r.GetGuid(0),
                    Name = r.GetString(1)
                });
            }

            return result;
        }
    }
}
