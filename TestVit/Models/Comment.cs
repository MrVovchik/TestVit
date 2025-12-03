namespace TestVit.Models
{
    public class Comment
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }    // пост
        public Guid AuthorId { get; set; }  // user
        public string Text { get; set; }    // текст комментария
        public DateTime CreatedAt { get; set; } // дата создания
    }
}
