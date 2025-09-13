// Application Layer - Use Cases
namespace BlogApp.Application
{
    using BlogApp.Domain;

    public interface INotificationService
    {
        void NotifyPostPublished(BlogPost post);
    }
}
