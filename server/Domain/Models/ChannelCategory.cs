using Domain.Models.Youtube;
using Newtonsoft.Json;
namespace Domain.Models
{
    public class ChannelCategory
    {
        [JsonIgnore]
        public string ChannelId { get; set; }
        [JsonIgnore]
        public YoutubeChannel Channel { get; set; }

        [JsonProperty("name")]
        public string CategoryName { get; set; }
        [JsonProperty("normalizedName")]
        public string CategoryNormalizedName { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
    }
}
