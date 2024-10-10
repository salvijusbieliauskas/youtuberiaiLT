using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Category
    {
        [Key]
        public string Name { get; set; } = string.Empty;
        [Key]
        public string NormalizedName { get; set; } = string.Empty;

        [JsonIgnore]
        public List<ChannelCategory> Channels { get; set; } = new List<ChannelCategory>();

    }
}
