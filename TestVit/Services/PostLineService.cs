using TestVit.DataAccess;
using TestVit.Models;
using TestVit.Models.DTOs;

namespace TestVit.Services;

public class PostsLineService : IPostsLineService
{
    private readonly UserDataAccess _users;
    private readonly SessionDataAccess _sessions;
    private readonly BlogPostDataAccess _posts;
    private readonly TagDataAccess _tags;
    private readonly CommentDataAccess _comments;
    private readonly LikeDataAccess _like;

    public PostsLineService(
        UserDataAccess users,
        SessionDataAccess sessions,
        BlogPostDataAccess posts,
        TagDataAccess tags,
        CommentDataAccess comments,
        LikeDataAccess like)
    {
        _users = users;
        _sessions = sessions;
        _posts = posts;
        _tags = tags;
        _comments = comments;
        _like = like;
    }

    //public async Task<List<BlogPost>> GetPostsByTagsAsync(IEnumerable<Guid> tagIds) // получаем посты
    //{
    //    return await _posts.GetPostsByTagsAsync(tagIds);
    //}

    public async Task<BlogPost?> GetPostByIdAsync(Guid postId) // получаем посты
    {
        return await _posts.GetPostByIdAsync(postId);
    }

    //public async Task<Guid> AddCommentAsync(Comment comment)
    //{
    //    return await _comments.CreateCommentAsync(comment);
    //}

    public async Task<Like> GetLikeAsync(Guid postId, Guid userId)
    {
        return await _like.GetLikeByPostIdByUserIdAsync(postId, userId);
    }

    public async Task<Guid> AddLikeAsync(Like like)
    {
        return await _like.CreateLikeAsync(like);
    }





    public async Task<List<BlogPost>> GetPostsByTagsAsync(List<string> tagNames)
    {
        var tags = await _tags.GetByNamesAsync(tagNames);

        var tagsIds = tags.Select(t => t.Id).ToList();

        return await _posts.GetPostsByTagsAsync(tagsIds);
    }

    public async Task<Guid> AddCommentAsync(Guid userId, AddCommentRequest req)
    {
        var post = await _posts.GetPostByIdAsync(req.PostId);
        if (post == null)
            throw new KeyNotFoundException("Post not found");

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            PostId = req.PostId,
            AuthorId = userId,
            Text = req.Text,
            CreatedAt = DateTime.UtcNow
        };

        await _comments.CreateCommentAsync(comment);

        return comment.Id;
    }

    public async Task<Guid> AddLikeAsync(Guid userId, AddLikeRequest req)
    {
        var post = await _posts.GetPostByIdAsync(req.PostId);
        if (post == null)
            throw new KeyNotFoundException("Post not found");

        var existing = await _like.GetLikeByPostIdByUserIdAsync(req.PostId, userId);
        if (existing != null)
            throw new InvalidOperationException("Already liked");

        var like = new Like
        {
            Id = Guid.NewGuid(),
            PostId = req.PostId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow
        };

        await _like.CreateLikeAsync(like);

        return like.Id;
    }
}
