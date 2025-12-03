namespace TestVit.Models
{
    public class BlogPost
    {
        public Guid Id { get; set; }
        public Guid AuthorId { get; set; }      // авток
        public string Title { get; set; }       // заголовок
        public string Body { get; set; }        // тело поста
        public DateTime CreatedAt { get; set; } // дата создания
    }
}
