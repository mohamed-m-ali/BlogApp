namespace BogAppOnion.Infrastructure.Services
{
    using BogAppOnion.Application.Interfaces;

    public class SystemDateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
