// Application Layer - Use Cases
namespace BlogApp.Application
{
    using BlogApp.Domain;

    // Use Cases
    public class BlogService
    {
        private readonly IBlogPostRepository _repository;
        private readonly INotificationService _notificationService;

        public BlogService(IBlogPostRepository repository, INotificationService notificationService)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        }

        public Guid CreatePost(string title, string content, string author)
        {
            var post = new BlogPost(title, content, author);
            _repository.Save(post);
            return post.Id;
        }

        public void PublishPost(Guid postId)
        {
            var post = _repository.GetById(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            post.Publish();
            _repository.Save(post);
            _notificationService.NotifyPostPublished(post);
        }

        public void UnpublishPost(Guid postId)
        {
            var post = _repository.GetById(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            post.Unpublish();
            _repository.Save(post);
        }

        public void UpdatePost(Guid postId, string title, string content)
        {
            var post = _repository.GetById(postId);
            if (post == null)
                throw new InvalidOperationException("Post not found");

            post.UpdateContent(title, content);
            _repository.Save(post);
        }

        public BlogPost GetPost(Guid postId)
        {
            return _repository.GetById(postId);
        }

        public IEnumerable<BlogPost> GetAllPosts()
        {
            return _repository.GetAll();
        }

        public IEnumerable<BlogPost> GetPublishedPosts()
        {
            return _repository.GetPublished();
        }

        public void DeletePost(Guid postId)
        {
            _repository.Delete(postId);
        }
    }
}
