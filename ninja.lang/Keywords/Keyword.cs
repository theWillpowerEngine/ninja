using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ninja.common;
using ninja.lang.keywords;

namespace ninja.lang
{
    internal class Keyword
    {
        internal static ConcurrentDictionary<string, Keyword> AllKeywords = new ConcurrentDictionary<string, Keyword>()
        {
            ["print"] = new PrintKeyword(), 
        };

        internal int Params = 0;

        internal virtual void Execute(List<string> parms, ref ConcurrentStack<NinjaField> stack)
        {
            if (_handler != null)
                stack = _handler(parms, stack);
            else
                throw new InvalidOperationException("Handler was null for a keyword but the default Execute was called");
        }

        protected Func<List<string>, ConcurrentStack<NinjaField>, ConcurrentStack<NinjaField>> _handler;

        internal Keyword(int parms)
        {
            Params = parms;
        }
    }
}
