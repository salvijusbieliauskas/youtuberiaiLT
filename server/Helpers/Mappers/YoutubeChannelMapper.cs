using Domain.Models.Youtube;
using Google.Apis.YouTube.v3.Data;

namespace Helpers.Mappers
{
    public static class YoutubeChannelMapper
    {
        public static YoutubeChannel ToYoutubeChannel(this ChannelListResponse item)
        {
            if (item is null)
            {
                return new YoutubeChannel();
            }
            var responseDetails = item.Items[0];

            if (responseDetails is null)
            {
                return new YoutubeChannel();
            }

            YoutubeChannel channelDetails = new YoutubeChannel()
            {
                Id = responseDetails.Id,
                Title = responseDetails.Snippet.Title,
                CustomUrl = $"https://www.youtube.com/{responseDetails.Snippet.CustomUrl}",
                Thumbnail = responseDetails.Snippet.Thumbnails.Medium.Url,
                Description = responseDetails.Snippet.Description,
                VideoCount = responseDetails.Statistics.VideoCount,
                SubscriberCount = responseDetails.Statistics.SubscriberCount,
            };

            return channelDetails;
        }
    }
}
