using ninja.common;
using ninja.common.interop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ninja.std
{
    public class Terminal : DotNetInteropper
    {
        private ConcurrentStack<NinjaField> HandleShit(string[] parms, ConcurrentStack<NinjaField> stack)
        {
            Console.WriteLine("Sheeyet");
            return stack;
        }
        
        public override List<InteropKeyword> GetKeywords()
        {
            var retVal = new List<InteropKeyword>();

            retVal.Add(new InteropKeyword("shit", 0)
            {
                Handler = new DotNetInteropHandler(HandleShit)
            });

            return retVal;
        }
    }
}
