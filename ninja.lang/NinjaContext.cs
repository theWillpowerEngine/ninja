using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang
{
    public class NinjaContext
    {
        private bool HasReadied = false;
        
        public readonly NinjaObject Object;
        public readonly Evaluator Evaluator;
        internal ConcurrentDictionary<string, Keyword> Keywords = new ConcurrentDictionary<string, Keyword>();

        public NinjaContext(NinjaObject obj)
        {
            Object = obj;
            Evaluator = new Evaluator(obj, this);
            Keywords = Evaluator.GetKeywords();
        }

        public void Evaluate(string code = null)
        {
            if(!HasReadied)
            {
                HasReadied = true;
                var whenReady = Object.Get("when", "ready");
                if (!whenReady.IsUndefined)
                    Evaluator.Evaluate(whenReady.Value);
                else if(!string.IsNullOrEmpty(Object.Value))
                    Evaluator.Evaluate(Object.Value);
            }
        }
    }
}
