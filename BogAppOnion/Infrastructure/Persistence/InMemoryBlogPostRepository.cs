using BogAppOnion.Application.Interfaces;
using BogAppOnion.Core.Entities;

// ==================== INFRASTRUCTURE LAYER (Outermost) ====================
namespace BogAppOnion.Infrastructure.Persistence
{
    // Infrastructure Implementation - Data Access
    public class InMemoryBlogPostRepository : IBlogPostRepository
    {
        private readonly Dictionary<Guid, BlogPost> _posts = new Dictionary<Guid, BlogPost>();

        public Task<BlogPost> GetByIdAsync(Guid id)
        {
            _posts.TryGetValue(id, out var post);
            return Task.FromResult(post);
        }

        public Task<IEnumerable<BlogPost>> GetAllAsync()
        {
            return Task.FromResult<IEnumerable<BlogPost>>(_posts.Values.ToList());
        }

        public Task<IEnumerable<BlogPost>> GetPublishedAsync()
        {
            var published = _posts.Values.Where(p => p.Status == PostStatus.Published).ToList();
            return Task.FromResult<IEnumerable<BlogPost>>(published);
        }

        public Task<IEnumerable<BlogPost>> GetByAuthorAsync(string author)
        {
            var authorPosts = _posts.Values
                .Where(p => p.Author.Equals(author, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Task.FromResult<IEnumerable<BlogPost>>(authorPosts);
        }

        public Task SaveAsync(BlogPost post)
        {
            _posts[post.Id] = post;
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Guid id)
        {
            _posts.Remove(id);
            return Task.CompletedTask;
        }

        public Task<bool> ExistsAsync(Guid id)
        {
            return Task.FromResult(_posts.ContainsKey(id));
        }
    }
}
