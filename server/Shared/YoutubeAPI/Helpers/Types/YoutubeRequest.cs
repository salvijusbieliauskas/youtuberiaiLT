using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.YoutubeAPI.Helpers.Types
{
    internal class YoutubeRequestData
    {
        private Dictionary<string, object> data = new();

        public YoutubeRequestData() 
        {
            UpdateContext(ClientType.WEB);
        }

        public YoutubeRequestData AddValue(string key, object value)
        {
            if (data.ContainsKey(key)) data[key] = value;
            else data.Add(key, value);
            return this;
        }

        private void UpdateContext(ClientType requestClient, string language = "en", string region = "US")
        {
            Dictionary<string, object> clientContext = new();
            clientContext.Add("hl", language);
            clientContext.Add("gl", region);
            switch (requestClient)
            {
                case ClientType.WEB:
                    clientContext.Add("browserName", "Safari");
                    clientContext.Add("browserVersion", "15.4");
                    clientContext.Add("clientName", "WEB");
                    clientContext.Add("clientVersion", "2.20230419.01.00");
                    clientContext.Add("deviceMake", "Apple");
                    clientContext.Add("osName", "Macintosh");
                    clientContext.Add("osVersion", "10_15_7");
                    clientContext.Add("platform", "DESKTOP");
                    clientContext.Add("originalUrl", "https://www.youtube.com");
                    clientContext.Add("userAgent",
                        "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/15.4 Safari/605.1.15,gzip(gfe)");
                    break;
                case ClientType.ANDROID:
                    clientContext.Add("clientName", "ANDROID");
                    clientContext.Add("clientVersion", "19.09.4");
                    clientContext.Add("osName", "Android");
                    clientContext.Add("osVersion", "11");
                    clientContext.Add("androidSdkVersion", 30);
                    clientContext.Add("platform", "MOBILE");
                    clientContext.Add("userAgent", "com.google.android.youtube/19.09.4 (Linux; U; Android 11) gzip");
                    break;
                case ClientType.IOS:
                    clientContext.Add("clientName", "IOS");
                    clientContext.Add("clientVersion", "18.15.1");
                    clientContext.Add("deviceMake", "Apple");
                    clientContext.Add("deviceModel", "iPhone14,5");
                    clientContext.Add("osName", "iOS");
                    clientContext.Add("osVersion", "15.6.0.19G71");
                    clientContext.Add("platform", "MOBILE");
                    break;
            }

            AddValue("context", new Dictionary<string, object>
            {
                ["client"] = clientContext
            });
        }

        public string GetJson(ClientType client, string language, string region)
        {
            UpdateContext(client, language, region);
            return JsonConvert.SerializeObject(data);
        }
    }
}
