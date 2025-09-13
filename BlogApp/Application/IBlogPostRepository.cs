// Application Layer - Use Cases
namespace BlogApp.Application
{
    using BlogApp.Domain;

    // Ports (Interfaces) - What the application needs
    public interface IBlogPostRepository
    {
        void Save(BlogPost post);
        BlogPost GetById(Guid id);
        IEnumerable<BlogPost> GetAll();
        IEnumerable<BlogPost> GetPublished();
        void Delete(Guid id);
    }
}
