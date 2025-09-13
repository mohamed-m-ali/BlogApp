// ==================== APPLICATION SERVICES LAYER ====================
namespace BogAppOnion.Application.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
