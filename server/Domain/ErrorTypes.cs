using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class ErrorTypes
    {
        public const string NotFound = "Not Found";
        public const  string Exists = "Entry already exists";
        public const string Unauthorized = "Unauthorized";
    }
}
