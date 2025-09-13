namespace BogAppOnion.Application.DTOs
{
    using BogAppOnion.Core.Entities;

    public class BlogPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? PublishedAt { get; set; }
        public PostStatus Status { get; set; }
        public int WordCount { get; set; }
        public int EstimatedReadTimeMinutes { get; set; }

        public static BlogPostDto FromEntity(BlogPost post)
        {
            var metrics = new Core.ValueObjects.PostMetrics(post.Content);
            return new BlogPostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                Author = post.Author,
                CreatedAt = post.CreatedAt,
                PublishedAt = post.PublishedAt,
                Status = post.Status,
                WordCount = metrics.WordCount,
                EstimatedReadTimeMinutes = metrics.EstimatedReadTimeMinutes
            };
        }
    }
}
