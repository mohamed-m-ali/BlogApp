// Presentation Layer - User Interface
namespace BlogApp.Presentation
{
    using BlogApp.Application;

    public class ConsoleUI
    {
        private readonly BlogService _blogService;

        public ConsoleUI(BlogService blogService)
        {
            _blogService = blogService ?? throw new ArgumentNullException(nameof(blogService));
        }

        public void Run()
        {
            Console.WriteLine("=== Blog Management System ===");
            Console.WriteLine("Demonstrating Hexagonal Architecture");
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
                            CreatePost();
                            break;
                        case "2":
                            ListAllPosts();
                            break;
                        case "3":
                            ListPublishedPosts();
                            break;
                        case "4":
                            PublishPost();
                            break;
                        case "5":
                            UnpublishPost();
                            break;
                        case "6":
                            UpdatePost();
                            break;
                        case "7":
                            DeletePost();
                            break;
                        case "8":
                            ViewPost();
                            break;
                        case "0":
                            Console.WriteLine("Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid option. Please try again.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error: {ex.Message}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void ShowMenu()
        {
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1. Create Post");
            Console.WriteLine("2. List All Posts");
            Console.WriteLine("3. List Published Posts");
            Console.WriteLine("4. Publish Post");
            Console.WriteLine("5. Unpublish Post");
            Console.WriteLine("6. Update Post");
            Console.WriteLine("7. Delete Post");
            Console.WriteLine("8. View Post Details");
            Console.WriteLine("0. Exit");
            Console.Write("\nYour choice: ");
        }

        private void CreatePost()
        {
            Console.WriteLine("\n=== Create New Post ===");
            Console.Write("Title: ");
            var title = Console.ReadLine();

            Console.Write("Content: ");
            var content = Console.ReadLine();

            Console.Write("Author: ");
            var author = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(content) && !string.IsNullOrWhiteSpace(author))
            {
                var postId = _blogService.CreatePost(title, content, author);
                Console.WriteLine($"✅ Post created successfully! ID: {postId}");
            }
            else
            {
                Console.WriteLine("❌ All fields are required.");
            }
        }

        private void ListAllPosts()
        {
            Console.WriteLine("\n=== All Posts ===");
            var posts = _blogService.GetAllPosts().ToList();

            if (!posts.Any())
            {
                Console.WriteLine("No posts found.");
                return;
            }

            foreach (var post in posts)
            {
                Console.WriteLine($"ID: {post.Id}");
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Author: {post.Author}");
                Console.WriteLine($"Status: {post.Status}");
                Console.WriteLine($"Created: {post.CreatedAt:yyyy-MM-dd HH:mm}");
                if (post.PublishedAt.HasValue)
                    Console.WriteLine($"Published: {post.PublishedAt:yyyy-MM-dd HH:mm}");
                Console.WriteLine(new string('-', 50));
            }
        }

        private void ListPublishedPosts()
        {
            Console.WriteLine("\n=== Published Posts ===");
            var posts = _blogService.GetPublishedPosts().ToList();

            if (!posts.Any())
            {
                Console.WriteLine("No published posts found.");
                return;
            }

            foreach (var post in posts)
            {
                Console.WriteLine($"Title: {post.Title}");
                Console.WriteLine($"Author: {post.Author}");
                Console.WriteLine($"Published: {post.PublishedAt:yyyy-MM-dd HH:mm}");
                Console.WriteLine($"Content: {post.Content.Substring(0, Math.Min(100, post.Content.Length))}...");
                Console.WriteLine(new string('-', 50));
            }
        }

        private void PublishPost()
        {
            Console.WriteLine("\n=== Publish Post ===");
            var postId = GetPostIdFromUser();
            if (postId.HasValue)
            {
                _blogService.PublishPost(postId.Value);
                Console.WriteLine("✅ Post published successfully!");
            }
        }

        private void UnpublishPost()
        {
            Console.WriteLine("\n=== Unpublish Post ===");
            var postId = GetPostIdFromUser();
            if (postId.HasValue)
            {
                _blogService.UnpublishPost(postId.Value);
                Console.WriteLine("✅ Post unpublished successfully!");
            }
        }

        private void UpdatePost()
        {
            Console.WriteLine("\n=== Update Post ===");
            var postId = GetPostIdFromUser();
            if (!postId.HasValue) return;

            Console.Write("New Title: ");
            var title = Console.ReadLine();

            Console.Write("New Content: ");
            var content = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(content))
            {
                _blogService.UpdatePost(postId.Value, title, content);
                Console.WriteLine("✅ Post updated successfully!");
            }
            else
            {
                Console.WriteLine("❌ Title and content are required.");
            }
        }

        private void DeletePost()
        {
            Console.WriteLine("\n=== Delete Post ===");
            var postId = GetPostIdFromUser();
            if (postId.HasValue)
            {
                _blogService.DeletePost(postId.Value);
                Console.WriteLine("✅ Post deleted successfully!");
            }
        }

        private void ViewPost()
        {
            Console.WriteLine("\n=== View Post Details ===");
            var postId = GetPostIdFromUser();
            if (!postId.HasValue) return;

            var post = _blogService.GetPost(postId.Value);
            if (post == null)
            {
                Console.WriteLine("❌ Post not found.");
                return;
            }

            Console.WriteLine($"Title: {post.Title}");
            Console.WriteLine($"Author: {post.Author}");
            Console.WriteLine($"Status: {post.Status}");
            Console.WriteLine($"Created: {post.CreatedAt:yyyy-MM-dd HH:mm}");
            if (post.PublishedAt.HasValue)
                Console.WriteLine($"Published: {post.PublishedAt:yyyy-MM-dd HH:mm}");
            Console.WriteLine($"\nContent:\n{post.Content}");
        }

        private Guid? GetPostIdFromUser()
        {
            Console.Write("Enter Post ID: ");
            var input = Console.ReadLine();

            if (Guid.TryParse(input, out var postId))
            {
                return postId;
            }

            Console.WriteLine("❌ Invalid Post ID format.");
            return null;
        }
    }
}
