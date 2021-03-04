using ninja.lang;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang.providers
{
    internal class InteropProvider
    {
        private static readonly ConcurrentDictionary<string, Interopper> Std = new ConcurrentDictionary<string, Interopper>();
        
        internal Interopper Get(string name)
        {
            //Standard library?
            if (Std.ContainsKey(name.ToLower()))
                return Std[name.ToLower()];

            return null;
        }
    }
}
