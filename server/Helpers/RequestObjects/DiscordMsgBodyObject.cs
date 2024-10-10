using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.RequestObjects
{
    public class DiscordMsgBodyObject
    {
        public string Message { get; set; } = string.Empty;
        public List<string>? Categories { get; set; }
    }
}
