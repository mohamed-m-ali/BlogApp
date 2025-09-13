namespace BogAppOnion.Application.DTOs
{
    // Data Transfer Objects for crossing layer boundaries
    public class CreatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string Author { get; set; }
    }
}
