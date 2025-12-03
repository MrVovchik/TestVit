namespace TestVit.Models.DTOs
{
    public class CreateBlogPostRequest
    {
        public string Title { get; set; }
        public string Body { get; set; }
        public List<Guid> TagIds { get; set; } = new();
    }
}
