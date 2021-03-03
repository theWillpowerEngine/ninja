﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ninja.lang
{
    public class Evaluator
    {
        protected readonly NinjaObject Object;
        protected readonly NinjaContext Context;
        private ConcurrentDictionary<string, Keyword> Keywords
        {
            get => Context.Keywords;
        }


        internal ConcurrentDictionary<string, Keyword> GetKeywords()
        {
            var retVal = new ConcurrentDictionary<string, Keyword>(Keyword.AllKeywords);

            return retVal;
        }

        protected Stack<string> FancyStringSplit(string code)
        {
            var retVal = new Stack<string>();
            var work = "";
            var stringChar = "";

            for(var i=0; i<code.Length; i++)
            {
                var c = code[i];

                if (stringChar != "")
                {
                    if(c == stringChar[0])
                    {
                        retVal.Push(work);
                        work = "";
                        stringChar = "";
                    } else 
                        work += c;
                    continue;
                }


                switch(c)
                {
                    case ' ':
                    case '\t':
                    case '\r':
                    case '\n':
                        if (work != "")
                            retVal.Push(work);
                        work = "";
                        break;

                    case '"':
                    case '\'':
                    case '`':
                        if (work != "")
                            retVal.Push(work);
                        work = "";
                        stringChar = c.ToString();
                        break;

                    default:
                        work += c;
                        break;
                }
            }

            if (work != "")
                retVal.Push(work);

            return new Stack<string>(retVal);
        }

        protected string ProcessValue(string value)
        {
            if(value.StartsWith('$'))
            {
                return Object.GetValue(value.TrimStart('$')).Value;
            } 
            else
            {
                return value;
            }
        }

        public void Evaluate(string code)
        {
            var codeStack = FancyStringSplit(code);

            while(codeStack.Count > 0)
            {
                var kw = codeStack.Pop();
                
                if(Keywords.ContainsKey(kw.ToLower()))
                {
                    var handler = Keywords[kw.ToLower()];
                    var parms = new List<string>();

                    for (var i = 0; i < handler.Params; i++)
                        parms.Add(ProcessValue(codeStack.Pop()));

                    handler.Execute(parms);
                }
            }
        }

        public Evaluator(NinjaObject obj, NinjaContext context)
        {
            Object = obj;
            Context = context;
        }
    }
}
