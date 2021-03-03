using System;
using System.Collections.Concurrent;

namespace ninja.common
{
    public static class Extensions
    {
        public static void Upsert<T, T2>(this ConcurrentDictionary<T, T2> dict, T key, T2 val)
        {
            dict.AddOrUpdate(key, val, (key, old) => val);
        }
    }
}
