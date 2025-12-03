using Microsoft.AspNetCore.Mvc;
using TestVit.DataAccess;
using TestVit.Helpers;
using TestVit.Models.DTOs;
using TestVit.Services;

namespace TestVit.Controllers
{
    [ApiController]
    [Route("api/blog")]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostsService _service;
        private readonly SessionDataAccess _session;

        public BlogPostsController(IBlogPostsService service, SessionDataAccess session)
        {
            _service = service;
            _session = session;
        }

        [HttpPost("add-tag")]
        public async Task<IActionResult> AddTag([FromBody] CreateTagRequest req)
        {
            var userId = await ValidationHelper.ValidateAuth(this, _session);
            if (userId == null)
                return Unauthorized(new { error = "Invalid or expired token" });

            //var tagId = await _service.AddTagAsync(req.Name);

            var tagId = await _service.AddTagAsync(req.Name);

            return Ok(new { id = tagId });
        }

        [HttpPost("add-post")]
        public async Task<IActionResult> AddPost([FromBody] CreateBlogPostRequest req)
        {
            // auth
            var userId = await ValidationHelper.ValidateAuth(this, _session);
            if (userId == null)
                return Unauthorized(new { error = "Invalid or expired token" });

            var postId = await _service.AddPostAsync((Guid)userId, req);

            return Ok(new { id = postId });
        }
    }
}

