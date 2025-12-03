namespace TestVit.Models
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }         // лайк к какому посту
        public Guid UserId { get; set; }         // кто лайкнул
        public DateTime CreatedAt { get; set; }  // дата лайка
    }
}
