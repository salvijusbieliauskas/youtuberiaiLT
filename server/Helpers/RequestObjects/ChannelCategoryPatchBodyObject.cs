using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.RequestObjects
{
    public class ChannelCategoryPatchBodyObject
    {
        public PatchMethod PatchMethod { get; set; }
        public string Category { get; set; }
    }
}
