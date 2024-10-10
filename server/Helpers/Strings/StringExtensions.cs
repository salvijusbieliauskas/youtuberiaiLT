using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Strings
{
    public static class StringExtensions
    {
        public static string MyNormalize(this string input, NormalizationForm normalizationForm = NormalizationForm.FormD) 
        {
            string normalizedString = input.Normalize(normalizationForm);

            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in normalizedString)
            {
                UnicodeCategory unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }
            return stringBuilder.ToString().ToUpperInvariant(); 
        }
    }
}
