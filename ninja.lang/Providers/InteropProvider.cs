using ninja.lang;
using ninja.std;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang.providers
{
    internal class InteropProvider
    {
        private NinjaContext _nc;
        
        private static readonly ConcurrentDictionary<string, Interopper> Std = new ConcurrentDictionary<string, Interopper>();
        
        internal Interopper Get(string name)
        {
            //Standard library?
            if (Std.ContainsKey(name.ToLower()))
                return Std[name.ToLower()];

            return null;
        }

        private void CreateDefaultInterops()
        {
            Std.TryAdd("terminal", new Interopper(_nc, "terminal", new Terminal()));
        }

        internal InteropProvider(NinjaContext nc)
        {
            _nc = nc;

            CreateDefaultInterops();
        }
    }
}
