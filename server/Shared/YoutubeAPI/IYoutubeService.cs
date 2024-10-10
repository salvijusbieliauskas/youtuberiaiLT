using Google.Apis.YouTube.v3.Data;

namespace Services.YoutubeAPI
{
    public interface IYoutubeService
    {
        Task<ChannelListResponse?> getChannelById(string channelId);
        Task<ChannelListResponse?> getChannelByHandle(string channelHandle);
        Task<ChannelListResponse?> getChannel(string identifier);
    }
}
