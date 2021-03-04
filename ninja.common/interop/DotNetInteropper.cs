using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ninja.common.interop
{
    public delegate ConcurrentStack<NinjaField> DotNetInteropHandler(string[] parms, ConcurrentStack<NinjaField> stack);
    
    public abstract class DotNetInteropper
    {
        public abstract List<InteropKeyword> GetKeywords();
    }
}
