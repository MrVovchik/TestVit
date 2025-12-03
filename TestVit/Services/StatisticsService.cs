using TestVit.DataAccess;

namespace TestVit.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly UserDataAccess _users;
        private readonly SessionDataAccess _sessions;
        private readonly BlogPostDataAccess _posts;
        private readonly TagDataAccess _tags;
        private readonly CommentDataAccess _comments;
        private readonly LikeDataAccess _like;
        private readonly BlogPostTegDataAccess _blogPostTeg;
        private readonly BlogPostDataAccess _blogPost;

        public StatisticsService(
            UserDataAccess users,
            SessionDataAccess sessions,
            BlogPostDataAccess posts,
            TagDataAccess tags,
            CommentDataAccess comments,
            LikeDataAccess like,
            BlogPostTegDataAccess blogpostteg,
            BlogPostDataAccess blogPost)
        {
            _users = users;
            _sessions = sessions;
            _posts = posts;
            _tags = tags;
            _comments = comments;
            _like = like;
            _blogPostTeg = blogpostteg;
            _blogPost = blogPost;
        }

        // количество постов по датам
        public async Task<Dictionary<DateTime, int>> GetPostsCountByDateAsync()
        {
            var posts = await _posts.GetAllAsync();

            return posts
                .GroupBy(p => p.CreatedAt.Date)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // количество постов по пользователям
        public async Task<Dictionary<Guid, int>> GetPostsCountByUserAsync()
        {
            var posts = await _posts.GetAllAsync();

            return posts
                .GroupBy(p => p.AuthorId)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // количество постов по тэгам
        public async Task<Dictionary<string, int>> GetPostsCountByTagsAsync()
        {            
            var postsTags = await _blogPostTeg.GetAllPostTagsAsync();

            if (postsTags.Count == 0)
                return new Dictionary<string, int>();

            var tagIds = postsTags
                .Select(pt => pt.TagId)
                .Distinct()
                .ToList();

            var tags = await _tags.GetByIdsAsync(tagIds);

            var grouped = postsTags
                .GroupBy(pt => pt.TagId)
                .ToDictionary(g => g.Key, g => g.Count());

            var result = new Dictionary<string, int>();

            foreach (var tag in tags)
            {
                grouped.TryGetValue(tag.Id, out var count);
                result[tag.Name] = count;
            }

            return result;
        }

        // количество лайков по датам
        public async Task<Dictionary<DateTime, int>> GetLikesCountByDateAsync()
        {
            var likes = await _like.GetAllAsync();

            return likes
                .GroupBy(l => l.CreatedAt.Date)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        // количество лайков по постам
        public async Task<Dictionary<Guid, int>> GetLikesCountByPostAsync()
        {
            var likes = await _like.GetAllAsync();

            return likes
                .GroupBy(l => l.PostId)
                .ToDictionary(g => g.Key, g => g.Count());
        }
    }
}
