// ==================== CORE LAYER (Innermost) ====================
namespace BogAppOnion.Core.Entities
{
    // Domain Entities - Pure business objects
    public class BlogPost
    {
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Author { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? PublishedAt { get; private set; }
        public PostStatus Status { get; private set; }

        private BlogPost() { } // For EF Core or serialization

        public BlogPost(string title, string content, string author)
        {
            Id = Guid.NewGuid();
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
            Author = author ?? throw new ArgumentNullException(nameof(author));
            CreatedAt = DateTime.UtcNow;
            Status = PostStatus.Draft;
        }

        public void Publish()
        {
            if (Status == PostStatus.Published)
                throw new InvalidOperationException("Post is already published");

            Status = PostStatus.Published;
            PublishedAt = DateTime.UtcNow;
        }

        public void Unpublish()
        {
            if (Status != PostStatus.Published)
                throw new InvalidOperationException("Post is not published");

            Status = PostStatus.Draft;
            PublishedAt = null;
        }

        public void UpdateContent(string title, string content)
        {
            if (Status == PostStatus.Published)
                throw new InvalidOperationException("Cannot update published post without unpublishing first");

            Title = title ?? throw new ArgumentNullException(nameof(title));
            Content = content ?? throw new ArgumentNullException(nameof(content));
        }
    }
}
