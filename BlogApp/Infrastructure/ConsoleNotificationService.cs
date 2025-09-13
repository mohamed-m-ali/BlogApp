// Infrastructure Layer - Adapters (Implementations)
namespace BlogApp.Infrastructure
{
    using BlogApp.Domain;
    using BlogApp.Application;

    // Notification Adapter - Console Implementation
    public class ConsoleNotificationService : INotificationService
    {
        public void NotifyPostPublished(BlogPost post)
        {
            Console.WriteLine($"🔔 NOTIFICATION: Post '{post.Title}' by {post.Author} has been published!");
        }
    }
}
