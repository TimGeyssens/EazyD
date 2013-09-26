using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EazyD.Extensions;
using EazyD.Interfaces;

namespace EazyD.Providers
{
    public class AutoGenKey: IAutoGenKey
    {
        public string Generate(string value)
        {
            var val = value.ToCamelCase();
            if (val.Length > 50)
                val = val.Substring(0, 50);
            return val;
        }
    }
}