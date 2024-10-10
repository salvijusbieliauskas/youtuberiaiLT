using Domain.Models.Youtube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Stores
{
    public class DailyChannelStore
    {
        private object _lock = new object();
        public YoutubeChannel TodaysChannel { get; private set; } = new();
        public YoutubeChannel YesterdaysChannel { get; private set; } = new();

        public void SetSelectedChannel(YoutubeChannel selectedChannel)
        {
            lock (_lock)
            { 
                YesterdaysChannel = TodaysChannel;
                TodaysChannel = selectedChannel;
            }
        }
    }
}
