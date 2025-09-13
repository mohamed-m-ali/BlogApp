namespace BogAppOnion.Core.Exceptions
{
    public class BlogPostValidationException : Exception
    {
        public BlogPostValidationException(string message) : base(message)
        {
        }
    }
}
