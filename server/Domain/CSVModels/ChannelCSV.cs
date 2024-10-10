using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CSVModels
{
    public class ChannelCSV(string Handle, string Categories)
    {
        public string Handle { get; set; } = Handle.Trim();
        public List<string> Categories { get; set; } = Categories.Trim().Split(",").ToList();
    }
}
