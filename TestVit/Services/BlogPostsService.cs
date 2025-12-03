using TestVit.DataAccess;
using TestVit.Models;
using TestVit.Models.DTOs;

namespace TestVit.Services
{
    public class BlogPostsService : IBlogPostsService
    {
        private readonly BlogPostDataAccess _posts;
        private readonly TagDataAccess _tags;

        public BlogPostsService(BlogPostDataAccess posts, TagDataAccess tags)
        {
            _posts = posts;
            _tags = tags;
        }

        public async Task<Guid> AddPostAsync(Guid authorId, CreateBlogPostRequest req)
        {
            var postId = await _posts.CreatePostAsync(authorId, req.Title, req.Body);

            if (req.TagIds != null && req.TagIds.Count > 0)
            {
                foreach (var tagId in req.TagIds)
                {
                    await _posts.AddTagToPostAsync(postId, tagId);
                }
            }

            return postId;
        }

        public async Task<Guid> AddTagAsync(string name)
        {
            return await _tags.CreateTagAsync(name);
        }
    }
}
