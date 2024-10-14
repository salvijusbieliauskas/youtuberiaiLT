using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Configuration;
using Domain.Models.Youtube;
using Services.YoutubeAPI.Helpers;
using Services.YoutubeAPI.Helpers.Wrappers;
using System.Diagnostics;

namespace Services.YoutubeAPI
{
    public class YoutubeService : IYoutubeService
    {
        private readonly IConfiguration _config;
        public YoutubeService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<YoutubeChannel?> getChannelById(string channelId)
        {
            Result<YoutubeChannel> result = await Utils.GetChannelAsync(channelId);
            if (!result)
            {
                Debug.WriteLine("Failed fetching channel: " + result.Message);
                return null;
            }
            return result.Value;
        }
        public async Task<YoutubeChannel?> getChannelByHandle(string channelHandle)
        {
            Result<string> result = await Utils.GetChannelIdFromVanity(channelHandle);

            if (!result)
            {
                Debug.WriteLine("Failed fetching handle: " + result.Message);
                return null;
            }

            Result<YoutubeChannel> channelResult = await Utils.GetChannelAsync(result.Value);

            if (!channelResult)
            {
                Debug.WriteLine("Failed fetching channel: " + result.Message);
                return null;
            }

            return channelResult.Value;
        }

        public async Task<YoutubeChannel?> getChannel(string identifier)
        {
            if (identifier.Trim().StartsWith('@'))
                return await getChannelByHandle(identifier);

            return await getChannelById(identifier);
        }
    }
}
