using BogAppOnion.Core.Entities;

namespace BogAppOnion.Infrastructure.Services
{
    using BogAppOnion.Application.Interfaces;

    // Infrastructure Services
    public class ConsoleNotificationService : INotificationService
    {
        public Task NotifyPostPublishedAsync(BlogPost post)
        {
            Console.WriteLine($"🔔 PUBLISHED: '{post.Title}' by {post.Author} on {post.PublishedAt:yyyy-MM-dd HH:mm}");
            return Task.CompletedTask;
        }

        public Task NotifyPostUnpublishedAsync(BlogPost post)
        {
            Console.WriteLine($"📝 UNPUBLISHED: '{post.Title}' by {post.Author} - moved back to draft");
            return Task.CompletedTask;
        }
    }
}
