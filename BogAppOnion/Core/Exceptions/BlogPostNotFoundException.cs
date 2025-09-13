namespace BogAppOnion.Core.Exceptions
{
    // Domain-specific exceptions
    public class BlogPostNotFoundException : Exception
    {
        public BlogPostNotFoundException(Guid postId)
            : base($"Blog post with ID {postId} was not found.")
        {
        }
    }
}
