using TestVit.Models;
using TestVit.Models.DTOs;

namespace TestVit.Services
{
    public interface IPostsLineService
    {
        Task<List<BlogPost>> GetPostsByTagsAsync(List<string> tagNames);

        Task<Guid> AddCommentAsync(Guid userId, AddCommentRequest req);

        Task<Guid> AddLikeAsync(Guid userId, AddLikeRequest req);
    }
}
