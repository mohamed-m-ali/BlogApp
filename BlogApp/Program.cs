using System;
using System.Collections.Generic;
using System.Linq;

// Domain Layer - Core Business Logic
namespace BlogApp.Domain
{
}

// Application Layer - Use Cases
namespace BlogApp.Application
{
}

// Infrastructure Layer - Adapters (Implementations)
namespace BlogApp.Infrastructure
{
}

// Presentation Layer - User Interface
namespace BlogApp.Presentation
{
}

// Program Entry Point - Composition Root
namespace BlogApp
{
    using BlogApp.Application;
    using BlogApp.Infrastructure;
    using BlogApp.Presentation;

    class Program
    {
        static void Main(string[] args)
        {
            // Dependency Injection / Composition Root
            // This is where we wire up all dependencies
            var repository = new InMemoryBlogPostRepository();
            var notificationService = new ConsoleNotificationService();
            var blogService = new BlogService(repository, notificationService);
            var consoleUI = new ConsoleUI(blogService);

            // Run the application
            consoleUI.Run();
        }
    }
}