namespace BogAppOnion.Application.Services
{
    using BogAppOnion.Application.DTOs;
    using BogAppOnion.Application.Interfaces;
    using BogAppOnion.Core.Entities;
    using BogAppOnion.Core.Exceptions;

    // Application Services - Orchestrate business operations
    public class BlogPostApplicationService
    {
        private readonly IBlogPostRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IDateTimeProvider _dateTimeProvider;

        public BlogPostApplicationService(
            IBlogPostRepository repository,
            INotificationService notificationService,
            IDateTimeProvider dateTimeProvider)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
            _dateTimeProvider = dateTimeProvider ?? throw new ArgumentNullException(nameof(dateTimeProvider));
        }

        public async Task<Guid> CreatePostAsync(CreatePostRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new BlogPostValidationException("Title is required");
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new BlogPostValidationException("Content is required");
            if (string.IsNullOrWhiteSpace(request.Author))
                throw new BlogPostValidationException("Author is required");

            var post = new BlogPost(request.Title.Trim(), request.Content.Trim(), request.Author.Trim());
            await _repository.SaveAsync(post);

            return post.Id;
        }

        public async Task PublishPostAsync(Guid postId)
        {
            var post = await _repository.GetByIdAsync(postId);
            if (post == null)
                throw new BlogPostNotFoundException(postId);

            post.Publish();
            await _repository.SaveAsync(post);
            await _notificationService.NotifyPostPublishedAsync(post);
        }

        public async Task UnpublishPostAsync(Guid postId)
        {
            var post = await _repository.GetByIdAsync(postId);
            if (post == null)
                throw new BlogPostNotFoundException(postId);

            post.Unpublish();
            await _repository.SaveAsync(post);
            await _notificationService.NotifyPostUnpublishedAsync(post);
        }

        public async Task UpdatePostAsync(UpdatePostRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Title))
                throw new BlogPostValidationException("Title is required");
            if (string.IsNullOrWhiteSpace(request.Content))
                throw new BlogPostValidationException("Content is required");

            var post = await _repository.GetByIdAsync(request.Id);
            if (post == null)
                throw new BlogPostNotFoundException(request.Id);

            post.UpdateContent(request.Title.Trim(), request.Content.Trim());
            await _repository.SaveAsync(post);
        }

        public async Task DeletePostAsync(Guid postId)
        {
            if (!await _repository.ExistsAsync(postId))
                throw new BlogPostNotFoundException(postId);

            await _repository.DeleteAsync(postId);
        }

        public async Task<BlogPostDto> GetPostAsync(Guid postId)
        {
            var post = await _repository.GetByIdAsync(postId);
            if (post == null)
                throw new BlogPostNotFoundException(postId);

            return BlogPostDto.FromEntity(post);
        }

        public async Task<IEnumerable<BlogPostDto>> GetAllPostsAsync()
        {
            var posts = await _repository.GetAllAsync();
            return posts.Select(BlogPostDto.FromEntity);
        }

        public async Task<IEnumerable<BlogPostDto>> GetPublishedPostsAsync()
        {
            var posts = await _repository.GetPublishedAsync();
            return posts.Select(BlogPostDto.FromEntity);
        }

        public async Task<IEnumerable<BlogPostDto>> GetPostsByAuthorAsync(string author)
        {
            if (string.IsNullOrWhiteSpace(author))
                throw new ArgumentException("Author name is required", nameof(author));

            var posts = await _repository.GetByAuthorAsync(author.Trim());
            return posts.Select(BlogPostDto.FromEntity);
        }
    }
}
