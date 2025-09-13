// ==================== APPLICATION SERVICES LAYER ====================
namespace BogAppOnion.Application.Interfaces
{
    using BogAppOnion.Core.Entities;

    // External Service Contracts
    public interface INotificationService
    {
        Task NotifyPostPublishedAsync(BlogPost post);
        Task NotifyPostUnpublishedAsync(BlogPost post);
    }
}
