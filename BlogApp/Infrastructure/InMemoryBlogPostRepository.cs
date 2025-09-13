// Infrastructure Layer - Adapters (Implementations)
namespace BlogApp.Infrastructure
{
    using BlogApp.Domain;
    using BlogApp.Application;

    // Repository Adapter - In-Memory Implementation
    public class InMemoryBlogPostRepository : IBlogPostRepository
    {
        private readonly Dictionary<Guid, BlogPost> _posts = new Dictionary<Guid, BlogPost>();

        public void Save(BlogPost post)
        {
            _posts[post.Id] = post;
        }

        public BlogPost GetById(Guid id)
        {
            _posts.TryGetValue(id, out var post);
            return post;
        }

        public IEnumerable<BlogPost> GetAll()
        {
            return _posts.Values.ToList();
        }

        public IEnumerable<BlogPost> GetPublished()
        {
            return _posts.Values.Where(p => p.Status == PostStatus.Published).ToList();
        }

        public void Delete(Guid id)
        {
            _posts.Remove(id);
        }
    }
}
