using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using Microsoft.Extensions.Configuration;

namespace Services.YoutubeAPI
{
    public class YoutubeService : IYoutubeService
    {
        private readonly IConfiguration _config;
        private readonly string apiKey = string.Empty;
        private readonly string applicationName = string.Empty;
        public YoutubeService(IConfiguration config)
        {
            _config = config;
            apiKey = _config["GoogleAPIKey"];
            applicationName = _config["GoogleApplicationName"];
        }
        public async Task<ChannelListResponse?> getChannelById(string channelId)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = apiKey,
                ApplicationName = applicationName
            });

            var searchRequest = youtubeService.Channels.List(new[] { "snippet", "id", "statistics" });
            searchRequest.Id = channelId;

            var searchResponse = await searchRequest.ExecuteAsync();

            if (searchResponse is null || searchResponse.PageInfo.TotalResults <= 0)
            {
                return null;
            }

            return searchResponse;       

        }
        public async Task<ChannelListResponse?> getChannelByHandle(string channelHandle)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = apiKey,
                ApplicationName = applicationName
            });

            var searchRequest = youtubeService.Channels.List(new[] { "snippet", "id", "statistics" });
            searchRequest.ForHandle = channelHandle;

            var searchResponse = await searchRequest.ExecuteAsync();

            if (searchResponse is null || searchResponse.PageInfo.TotalResults <= 0)
            {
                return null;
            }

            return searchResponse;
        }

        public async Task<ChannelListResponse?> getChannel(string identifier)
        {
            var channelDetails = await getChannelByHandle(identifier);
            if (channelDetails is null) 
            {
                channelDetails = await getChannelById(identifier);
            }

            return channelDetails;
        }
    }
}
