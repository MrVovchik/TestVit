namespace TestVit.Services
{
    public interface IStatisticsService
    {
        public Task<Dictionary<DateTime, int>> GetPostsCountByDateAsync();

        public Task<Dictionary<Guid, int>> GetPostsCountByUserAsync();

        public Task<Dictionary<string, int>> GetPostsCountByTagsAsync();

        public Task<Dictionary<DateTime, int>> GetLikesCountByDateAsync();

        public Task<Dictionary<Guid, int>> GetLikesCountByPostAsync();
    }
}
