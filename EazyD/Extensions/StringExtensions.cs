using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EazyD.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this String input)
        {
            if (input == null) return "";

            var textInfo = new CultureInfo("en-US", false).TextInfo;
            var result = textInfo.ToTitleCase(input.Trim()).Replace(" ", "");

            return result;
        }
    }
}