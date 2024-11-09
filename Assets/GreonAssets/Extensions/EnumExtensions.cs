using System;
using System.Collections.Generic;
using System.Linq;

namespace GreonAssets.Extensions
{
    public static class EnumExtensions
    {
        public static Dictionary<string, TEnum> GetNameValueDictionary<TEnum>()
        {
            return Enum
                .GetValues(typeof(TEnum))
                .Cast<TEnum>()
                .ToDictionary(e => e.ToString(), e => e);
        }
    }
}
