using ninja.common;
using ninja.lang.providers;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ninja.lang
{
    public class NinjaContext
    {
        private bool HasReadied = false;

        internal List<Interopper> Interops = new List<Interopper>();
        internal readonly InteropProvider InteropProvider;
        
        public readonly NinjaObject Object;
        public readonly Evaluator Evaluator;
        internal ConcurrentDictionary<string, Keyword> Keywords = new ConcurrentDictionary<string, Keyword>();

        protected int RunningCount = 0;

        public NinjaContext(NinjaObject obj)
        {
            Object = obj;
            Evaluator = new Evaluator(obj, this);
            Keywords = Evaluator.GetKeywords();

            InteropProvider = new InteropProvider(this);
        }

        public void Evaluate()
        {
            //Step 1)  Apply uses
            var use = Object["use"];
            if(use?.Count > 0)
            {
                foreach(var u in use)
                {
                    var eles = u.Value.Split(new char[] { ' ', '\t', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var ele in eles)
                    {
                        var intop = InteropProvider.Get(ele);
                        if (intop == null)
                            throw new NullReferenceException("Interop Provider not found for " + u.Value);

                        intop.Install();
                    }
                }
            }

            //Step 2)  Apply type
            if(Is.MetaedObject(Object))
            {
                switch(Object.Value.ToLower())
                {
                    case "set":
                        Console.WriteLine("It's a set!");
                        break;

                    case "app":
                    case "application":
                        //Do nothing, same as default
                        break;

                    default:
                        throw new InvalidOperationException("Unknown application-object type: " + Object.Value);
                }
            }
            
            //Step 3)  Validate

            //Step 4)  Ready
            if(!HasReadied)
            {
                HasReadied = true;
                var whenReady = Object.Get("when", "ready");
                if (!whenReady.IsUndefined)
                    Evaluator.Evaluate(whenReady.Value);
                else if(!string.IsNullOrEmpty(Object.Value))
                    Evaluator.Evaluate(Object.Value);
            }

            //Step 5)  Running
            while (RunningCount > 0)
                Thread.Sleep(50);
        }
    }
}
