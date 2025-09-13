// ==================== APPLICATION SERVICES LAYER ====================
namespace BogAppOnion.Application.Interfaces
{
    using BogAppOnion.Core.Entities;

    // Repository Contracts (pointing inward to Core)
    public interface IBlogPostRepository
    {
        Task<BlogPost> GetByIdAsync(Guid id);
        Task<IEnumerable<BlogPost>> GetAllAsync();
        Task<IEnumerable<BlogPost>> GetPublishedAsync();
        Task<IEnumerable<BlogPost>> GetByAuthorAsync(string author);
        Task SaveAsync(BlogPost post);
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}
