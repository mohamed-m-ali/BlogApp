namespace BogAppOnion.Core.ValueObjects
{
    // Value Objects for domain concepts
    public class PostMetrics
    {
        public int WordCount { get; private set; }
        public int EstimatedReadTimeMinutes { get; private set; }

        public PostMetrics(string content)
        {
            WordCount = content?.Split(' ', StringSplitOptions.RemoveEmptyEntries).Length ?? 0;
            EstimatedReadTimeMinutes = Math.Max(1, WordCount / 200); // Assuming 200 words per minute
        }
    }
}
