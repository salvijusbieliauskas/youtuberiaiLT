using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Enums
{
    public enum SortOrder
    {
        Default = 0,
        [Display(Name = "By Handle Ascending")]
        ByHandleAscending = 1,
        [Display(Name = "By Handle Descending")]
        ByHandleDescending,
        [Display(Name = "By Sub Count Descending")]
        BySubCountAscending,
        [Display(Name = "By Sub Count Descending")]
        BySubCountDescending,
    }
}
