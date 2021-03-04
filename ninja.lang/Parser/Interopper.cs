using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang
{
    internal class Interopper
    {
        internal readonly NinjaContext Ninja;

        internal bool IsNinjaContext => Ninja != null;

        internal Interopper(NinjaContext nc)
        {
            Ninja = nc;
        }
    }
}
