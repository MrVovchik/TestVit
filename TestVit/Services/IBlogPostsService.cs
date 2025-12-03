using TestVit.Models.DTOs;

namespace TestVit.Services
{
    public interface IBlogPostsService
    {
        Task<Guid> AddTagAsync(string name);

        Task<Guid> AddPostAsync(Guid authorId, CreateBlogPostRequest req);
    }
}
