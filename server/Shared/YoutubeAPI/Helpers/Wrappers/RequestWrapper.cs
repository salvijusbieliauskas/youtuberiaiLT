using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using Services.YoutubeAPI.Helpers.Types;

namespace Services.YoutubeAPI.Helpers.Wrappers
{
    internal class RequestWrapper
    {
        private static RequestWrapper? instance = null;

        private const string ApiKey = @"AIzaSyAO_FJ2SlqU8Q4STEHLGCilw_Y9_11qcW8";
        private const int delayDeviation = 10;
        private const int baseDelayMilliseconds = 15;
        private static HttpClient httpClient = new HttpClient();

        private long lastRequest = 0;
        private Random random = new Random();
        private int delayMilliseconds = baseDelayMilliseconds;

        public static RequestWrapper GetInstance()
        {
            if (instance == null)
                instance = new RequestWrapper();

            return instance;
        }

        private RequestWrapper()
        {
        }

        internal async Task<Result<JObject>> RequestAsync(ClientType client, string endpoint, YoutubeRequestData postData,
            string language, string region)
        {
            HttpRequestMessage hrm = new(HttpMethod.Post,
                @$"https://www.youtube.com/youtubei/v1/{endpoint}?prettyPrint=false&key={ApiKey}");

            byte[] buffer = Encoding.UTF8.GetBytes(postData.GetJson(client, language, region));
            ByteArrayContent byteContent = new(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            hrm.Content = byteContent;

            hrm.Headers.Add("X-Youtube-Client-Name", ((int)client).ToString());
            hrm.Headers.Add("X-Youtube-Client-Version", client switch
            {
                ClientType.WEB => "2.20220809.02.00",
                ClientType.ANDROID => "19.09.4",
                ClientType.IOS => "19.09.4",
                var _ => ""
            });
            hrm.Headers.Add("Origin", "https://www.youtube.com");
            if (client == ClientType.ANDROID)
                hrm.Headers.Add("User-Agent", "com.google.android.youtube/19.09.4 (Linux; U; Android 11) gzip");

            await AssureDelay();

            HttpResponseMessage ytPlayerRequest = await httpClient.SendAsync(hrm);
            if (!ytPlayerRequest.IsSuccessStatusCode)
            {
                return Result.Err<JObject>("HTTP request failed").Append($"Status code: {ytPlayerRequest.StatusCode}").Append(await ytPlayerRequest.Content.ReadAsStringAsync());
            }
            return Result.Ok(JObject.Parse(await ytPlayerRequest.Content.ReadAsStringAsync()));
        }
        public async Task<Result<JObject>> BrowseAsync(string browseId, string? browseParams = null,
        string language = "lt", string region = "LT")
        {
            YoutubeRequestData postData = new YoutubeRequestData()
                .AddValue("browseId", browseId);

            if (browseParams is not null)
                postData.AddValue("params", browseParams);

            Result<JObject> browseResponse = await RequestAsync(ClientType.WEB,"browse", postData, language, region);

            return browseResponse;
        }
        public async Task<Result<string>> GetChannelIdFromVanity(string vanityUrl)
        {
            if (vanityUrl.StartsWith('@'))
                vanityUrl = "https://youtube.com/" + vanityUrl;
            else if (!vanityUrl.StartsWith("http"))
                vanityUrl = "https://youtube.com/c/" + vanityUrl;

            YoutubeRequestData postData = new YoutubeRequestData()
                .AddValue("url", vanityUrl);

            Result<JObject> browseResponse =
                await RequestAsync(ClientType.ANDROID, "navigation/resolve_url", postData, "en", "US");

            if (!browseResponse)
                return Result.Err<string>(browseResponse.Message);

            string? parsed = null;

            try
            {
                parsed = (string)browseResponse.Value["endpoint"]["browseEndpoint"]["browseId"];
            }
            catch
            {
                return Result.Err<string>("Youtube response invalid for vanity " + vanityUrl);
            }

            if (parsed is null) 
                return Result.Err<string>("Youtube response contained null ID" + vanityUrl);

            return Result.Ok(parsed);
        }
        private async Task AssureDelay()
        {
            long now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            long delayTarget = lastRequest + delayMilliseconds;
            long delta = delayTarget - now;
            if (delta > 0)
            {
                await Task.Delay((int)delta);
            }
            delayMilliseconds = random.Next(delayDeviation * 2 + 1) - delayDeviation + baseDelayMilliseconds;
            lastRequest = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        }
    }
}
