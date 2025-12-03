using Microsoft.AspNetCore.Mvc;
using TestVit.DataAccess;
using TestVit.Helpers;
using TestVit.Models.DTOs;
using TestVit.Services;

namespace TestVit.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostsLineController : ControllerBase
    {
        private readonly IPostsLineService _posts;
        private readonly SessionDataAccess _session;

        public PostsLineController(IPostsLineService posts, SessionDataAccess session)
        {
            _posts = posts;
            _session = session;
        }

        [HttpPost("by-tags")]
        public async Task<IActionResult> GetByTags([FromBody] PostsByTagsRequest req)
        {
            var userId = await ValidationHelper.ValidateAuth(this, _session);
            if (userId == null)
                return Unauthorized(new { error = "Invalid or expired token" });

            var posts = await _posts.GetPostsByTagsAsync(req.Tags);

            return Ok(posts);
        }

        [HttpPost("by-post/comment")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest req)
        {
            var userId = await ValidationHelper.ValidateAuth(this, _session);
            if (userId == null)
                return Unauthorized(new { error = "Invalid or expired token" });

            try
            {
                var id = await _posts.AddCommentAsync(userId.Value, req);
                return Ok(id);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { error = e.Message });
            }
        }

        [HttpPost("by-post/like")]
        public async Task<IActionResult> AddLike([FromBody] AddLikeRequest req)
        {
            var userId = await ValidationHelper.ValidateAuth(this, _session);
            if (userId == null)
                return Unauthorized(new { error = "Invalid or expired token" });

            try
            {
                var id = await _posts.AddLikeAsync(userId.Value, req);
                return Ok(id);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(new { error = e.Message });
            }
            catch (InvalidOperationException e)
            {
                return BadRequest(new { error = e.Message });
            }
        }
    }
}