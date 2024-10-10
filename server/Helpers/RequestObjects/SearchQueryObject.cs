using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.RequestObjects
{
    public class SearchQueryObject
    {
        public string? ChannelHandle { get; set; } = null;
        public SortOrder? SortOrder { get; set; } = null;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public List<string>? IncludeCategories { get; set; } = null;
        public List<string>? ExcludeCategories { get; set; } = null;
    }
}
