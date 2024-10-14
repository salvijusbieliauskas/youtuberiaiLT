using Domain.Models.Youtube;

namespace Services.YoutubeAPI
{
    public interface IYoutubeService
    {
        Task<YoutubeChannel?> getChannelById(string channelId);
        Task<YoutubeChannel?> getChannelByHandle(string channelHandle);
        Task<YoutubeChannel?> getChannel(string identifier);
    }
}
