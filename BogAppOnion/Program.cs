// ==================== CORE LAYER (Innermost) ====================
namespace BlogApp.Core.Entities
{
}

namespace BlogApp.Core.ValueObjects
{
}

namespace BlogApp.Core.Exceptions
{
}

// ==================== APPLICATION SERVICES LAYER ====================
namespace BlogApp.Application.Interfaces
{
}

namespace BlogApp.Application.DTOs
{
}

namespace BlogApp.Application.Services
{
}

// ==================== INFRASTRUCTURE LAYER (Outermost) ====================
namespace BlogApp.Infrastructure.Persistence
{
}

namespace BlogApp.Infrastructure.Services
{
}

// ==================== PRESENTATION LAYER (Outermost) ====================
namespace BlogApp.Presentation.ConsoleUI
{
}

// ==================== COMPOSITION ROOT ====================
namespace BlogApp
{
    using BogAppOnion.Application.Interfaces;
    using BogAppOnion.Application.Services;
    using BogAppOnion.Infrastructure.Persistence;
    using BogAppOnion.Infrastructure.Services;
    using BogAppOnion.Presentation;

    class Program
    {
        static async Task Main(string[] args)
        {
            // Composition Root - Onion Architecture Dependency Injection
            // Dependencies flow inward: Infrastructure → Application → Core

            // Infrastructure Layer (Outer)
            IBlogPostRepository repository = new InMemoryBlogPostRepository();
            INotificationService notificationService = new ConsoleNotificationService();
            IDateTimeProvider dateTimeProvider = new SystemDateTimeProvider();

            // Application Services Layer
            var blogService = new BlogPostApplicationService(
                repository,
                notificationService,
                dateTimeProvider);

            // Presentation Layer (Outer)
            var consoleInterface = new ConsoleBlogInterface(blogService);

            // Run the application
            await consoleInterface.RunAsync();
        }
    }
}