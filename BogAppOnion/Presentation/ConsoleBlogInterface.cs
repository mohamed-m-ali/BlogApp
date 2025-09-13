// ==================== PRESENTATION LAYER (Outermost) ====================
namespace BogAppOnion.Presentation
{
    using BogAppOnion.Core.Exceptions;
    using BogAppOnion.Application.DTOs;
    using BogAppOnion.Application.Services;

    // Presentation Layer - User Interface
    public class ConsoleBlogInterface
    {
        private readonly BlogPostApplicationService _blogService;

        public ConsoleBlogInterface(BlogPostApplicationService blogService)
        {
            _blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
        }

        public async Task RunAsync()
        {
            Console.WriteLine("=== Blog Management System - Onion Architecture ===");
            Console.WriteLine("🧅 Demonstrating layered architecture with dependency inversion");
            Console.WriteLine();

            while (true)
            {
                ShowMenu();
                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            await CreatePostAsync();
                            break;
                        case "2":
                            await ListAllPostsAsync();
                            break;
                        case "3":
                            await ListPublishedPostsAsync();
                            break;
                        case "4":
                            await PublishPostAsync();
                            break;
                        case "5":
                            await UnpublishPostAsync();
                            break;
                        case "6":
                            await UpdatePostAsync();
                            break;
                        case "7":
                            await DeletePostAsync();
                            break;
                        case "8":
                            await ViewPostDetailsAsync();
                            break;
                        case "9":
                            await ListPostsByAuthorAsync();
                            break;
                        case "0":
                            Console.WriteLine("👋 Goodbye!");
                            return;
                        default:
                            Console.WriteLine("❌ Invalid option. Please try again.");
                            break;
                    }
                }
                catch (BlogPostNotFoundException ex)
                {
                    Console.WriteLine($"❌ {ex.Message}");
                }
                catch (BlogPostValidationException ex)
                {
                    Console.WriteLine($"❌ Validation Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Unexpected Error: {ex.Message}");
                }

                Console.WriteLine("\n⏸️ Press any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("📋 Choose an option:");
            Console.WriteLine("1. 📝 Create Post");
            Console.WriteLine("2. 📄 List All Posts");
            Console.WriteLine("3. 🌐 List Published Posts");
            Console.WriteLine("4. 🚀 Publish Post");
            Console.WriteLine("5. 📝 Unpublish Post");
            Console.WriteLine("6. ✏️ Update Post");
            Console.WriteLine("7. 🗑️ Delete Post");
            Console.WriteLine("8. 👁️ View Post Details");
            Console.WriteLine("9. 👤 List Posts by Author");
            Console.WriteLine("0. 🚪 Exit");
            Console.Write("\n🎯 Your choice: ");
        }

        private async Task CreatePostAsync()
        {
            Console.WriteLine("\n📝 Create New Post");
            Console.WriteLine(new string('=', 30));

            Console.Write("📄 Title: ");
            var title = Console.ReadLine();

            Console.Write("✍️ Content: ");
            var content = Console.ReadLine();

            Console.Write("👤 Author: ");
            var author = Console.ReadLine();

            var request = new CreatePostRequest
            {
                Title = title,
                Content = content,
                Author = author
            };

            var postId = await _blogService.CreatePostAsync(request);
            Console.WriteLine($"✅ Post created successfully! ID: {postId}");
        }

        private async Task ListAllPostsAsync()
        {
            Console.WriteLine("\n📄 All Posts");
            Console.WriteLine(new string('=', 30));

            var posts = (await _blogService.GetAllPostsAsync()).ToList();

            if (!posts.Any())
            {
                Console.WriteLine("📭 No posts found.");
                return;
            }

            foreach (var post in posts.OrderByDescending(p => p.CreatedAt))
            {
                DisplayPostSummary(post);
            }
        }

        private async Task ListPublishedPostsAsync()
        {
            Console.WriteLine("\n🌐 Published Posts");
            Console.WriteLine(new string('=', 30));

            var posts = (await _blogService.GetPublishedPostsAsync()).ToList();

            if (!posts.Any())
            {
                Console.WriteLine("📭 No published posts found.");
                return;
            }

            foreach (var post in posts.OrderByDescending(p => p.PublishedAt))
            {
                DisplayPostSummary(post);
            }
        }

        private async Task PublishPostAsync()
        {
            Console.WriteLine("\n🚀 Publish Post");
            Console.WriteLine(new string('=', 30));

            var postId = await GetPostIdFromUserAsync();
            if (postId.HasValue)
            {
                await _blogService.PublishPostAsync(postId.Value);
                Console.WriteLine("✅ Post published successfully!");
            }
        }

        private async Task UnpublishPostAsync()
        {
            Console.WriteLine("\n📝 Unpublish Post");
            Console.WriteLine(new string('=', 30));

            var postId = await GetPostIdFromUserAsync();
            if (postId.HasValue)
            {
                await _blogService.UnpublishPostAsync(postId.Value);
                Console.WriteLine("✅ Post unpublished successfully!");
            }
        }

        private async Task UpdatePostAsync()
        {
            Console.WriteLine("\n✏️ Update Post");
            Console.WriteLine(new string('=', 30));

            var postId = await GetPostIdFromUserAsync();
            if (!postId.HasValue) return;

            Console.Write("📄 New Title: ");
            var title = Console.ReadLine();

            Console.Write("✍️ New Content: ");
            var content = Console.ReadLine();

            var request = new UpdatePostRequest
            {
                Id = postId.Value,
                Title = title,
                Content = content
            };

            await _blogService.UpdatePostAsync(request);
            Console.WriteLine("✅ Post updated successfully!");
        }

        private async Task DeletePostAsync()
        {
            Console.WriteLine("\n🗑️ Delete Post");
            Console.WriteLine(new string('=', 30));

            var postId = await GetPostIdFromUserAsync();
            if (postId.HasValue)
            {
                Console.Write($"⚠️ Are you sure you want to delete this post? (y/N): ");
                var confirmation = Console.ReadLine();

                if (confirmation?.ToLower() == "y" || confirmation?.ToLower() == "yes")
                {
                    await _blogService.DeletePostAsync(postId.Value);
                    Console.WriteLine("✅ Post deleted successfully!");
                }
                else
                {
                    Console.WriteLine("❌ Deletion cancelled.");
                }
            }
        }

        private async Task ViewPostDetailsAsync()
        {
            Console.WriteLine("\n👁️ View Post Details");
            Console.WriteLine(new string('=', 30));

            var postId = await GetPostIdFromUserAsync();
            if (!postId.HasValue) return;

            var post = await _blogService.GetPostAsync(postId.Value);
            DisplayPostDetails(post);
        }

        private async Task ListPostsByAuthorAsync()
        {
            Console.WriteLine("\n👤 Posts by Author");
            Console.WriteLine(new string('=', 30));

            Console.Write("👤 Author name: ");
            var author = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("❌ Author name is required.");
                return;
            }

            var posts = (await _blogService.GetPostsByAuthorAsync(author)).ToList();

            if (!posts.Any())
            {
                Console.WriteLine($"📭 No posts found for author '{author}'.");
                return;
            }

            Console.WriteLine($"📚 Found {posts.Count} post(s) by '{author}':");
            foreach (var post in posts.OrderByDescending(p => p.CreatedAt))
            {
                DisplayPostSummary(post);
            }
        }

        private async Task<Guid?> GetPostIdFromUserAsync()
        {
            Console.Write("🆔 Enter Post ID: ");
            var input = Console.ReadLine();

            if (Guid.TryParse(input, out var postId))
            {
                return postId;
            }

            Console.WriteLine("❌ Invalid Post ID format.");
            return null;
        }

        private void DisplayPostSummary(BlogPostDto post)
        {
            var statusIcon = post.Status == Core.Entities.PostStatus.Published ? "🌐" : "📝";
            Console.WriteLine($"{statusIcon} {post.Title}");
            Console.WriteLine($"   👤 Author: {post.Author} | 🆔 ID: {post.Id}");
            Console.WriteLine($"   📅 Created: {post.CreatedAt:yyyy-MM-dd HH:mm} | Status: {post.Status}");
            if (post.PublishedAt.HasValue)
                Console.WriteLine($"   🚀 Published: {post.PublishedAt:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"   📊 {post.WordCount} words | ⏱️ {post.EstimatedReadTimeMinutes} min read");
            Console.WriteLine(new string('-', 60));
        }

        private void DisplayPostDetails(BlogPostDto post)
        {
            var statusIcon = post.Status == Core.Entities.PostStatus.Published ? "🌐" : "📝";
            Console.WriteLine($"\n{statusIcon} {post.Title}");
            Console.WriteLine(new string('=', post.Title.Length + 2));
            Console.WriteLine($"👤 Author: {post.Author}");
            Console.WriteLine($"🆔 ID: {post.Id}");
            Console.WriteLine($"📅 Created: {post.CreatedAt:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"📊 Status: {post.Status}");
            if (post.PublishedAt.HasValue)
                Console.WriteLine($"🚀 Published: {post.PublishedAt:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"📏 Word Count: {post.WordCount}");
            Console.WriteLine($"⏱️ Estimated Read Time: {post.EstimatedReadTimeMinutes} minutes");
            Console.WriteLine("\n📄 Content:");
            Console.WriteLine(new string('-', 40));
            Console.WriteLine(post.Content);
            Console.WriteLine(new string('-', 40));
        }
    }
}
