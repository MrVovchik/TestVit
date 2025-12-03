namespace TestVit.Models.DTOs
{
    public class AddCommentRequest
    {
        public Guid PostId { get; set; }
        public string Text { get; set; }
    }
}
