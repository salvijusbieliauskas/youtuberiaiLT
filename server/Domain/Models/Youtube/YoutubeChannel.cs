using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Youtube
{
    public class YoutubeChannel
    {
        [Key]
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string CustomUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Thumbnail { get; set; } = string.Empty;
        public ulong? SubscriberCount { get; set; } = 0;
        public ulong? VideoCount { get; set; } = 0;
        public DateOnly LastUpdatedAt { get; private set; } = DateOnly.FromDateTime(DateTime.UtcNow);

        public List<ChannelCategory> Categories { get; set; } = new List<ChannelCategory>();

        public void UpdateDate()
        {
            LastUpdatedAt = DateOnly.FromDateTime(DateTime.UtcNow);
        }
    }
}
