using Microsoft.AspNetCore.Mvc;
using TestVit.Models.DTOs;
using TestVit.Services;

namespace TestVit.Controllers
{
    [ApiController]
    [Route("api/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("posts/by-date")]
        public async Task<IActionResult> GetPostsCountByDate()
        {
            var dict = await _statisticsService.GetPostsCountByDateAsync();
            var result = dict.Select(x => new DateCountRequest
            {
                Date = x.Key,
                Count = x.Value
            }).ToList();

            return Ok(result);
        }

        [HttpGet("posts/by-user")]
        public async Task<IActionResult> GetPostsCountByUser()
        {
            var dict = await _statisticsService.GetPostsCountByUserAsync();
            var result = dict.Select(x => new UserCountRequest
            {
                UserId = x.Key,
                Count = x.Value
            }).ToList();

            return Ok(result);
        }

        [HttpGet("posts/by-tags")]
        public async Task<IActionResult> GetPostsCountByTags()
        {
            var dict = await _statisticsService.GetPostsCountByTagsAsync();
            var result = dict.Select(x => new TagCountRequest
            {
                TagName = x.Key,
                Count = x.Value
            }).ToList();

            return Ok(result);
        }

        [HttpGet("likes/by-date")]
        public async Task<IActionResult> GetLikesCountByDate()
        {
            var dict = await _statisticsService.GetLikesCountByDateAsync();
            var result = dict.Select(x => new DateCountRequest
            {
                Date = x.Key,
                Count = x.Value
            }).ToList();

            return Ok(result);
        }

        [HttpGet("likes/by-post")]
        public async Task<IActionResult> GetLikesCountByPost()
        {
            var dict = await _statisticsService.GetLikesCountByPostAsync();
            var result = dict.Select(x => new PostCountRequest
            {
                PostId = x.Key,
                Count = x.Value
            }).ToList();

            return Ok(result);
        }
    }
}
