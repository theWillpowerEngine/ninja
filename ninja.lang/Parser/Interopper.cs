using ninja.common.interop;
using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang
{
    internal class Interopper
    {
        internal readonly NinjaContext _nc;
        internal readonly string _name;
        
        internal readonly NinjaContext Ninja;
        internal readonly DotNetInteropper DotNet;
        
        internal bool IsNinjaContext => Ninja != null;
        internal bool IsDotNet => DotNet != null;

        internal Interopper(NinjaContext ctx, string name, NinjaContext nc)
        {
            Ninja = nc;
            _nc = ctx;
            _name = name;
        }
        internal Interopper(NinjaContext ctx, string name, DotNetInteropper dn)
        {
            DotNet = dn;
            _nc = ctx;
            _name = name;
        }

        internal void Install()
        {
            if(IsDotNet)
            {
                var kws = DotNet.GetKeywords();
                foreach(var kw in kws)
                {
                    if(!(kw.Handler is DotNetInteropHandler))
                        throw new InteropException($"DotNet interop keyword's handler is not a DotNetInteropHandler, library {_name}, keyword {kw}");
                    if (_nc.Keywords.ContainsKey(kw.Word))
                        throw new InteropException($"Keyword collision, library {_name}, keyword {kw}");

                    _nc.Keywords.TryAdd(kw.Word, new Keyword(kw.ParamCount, (parms, stack) => (kw.Handler as DotNetInteropHandler)(parms.ToArray(), stack)));
                }

                return;
            }

            throw new InteropException($"Library {_name} is of a type that the Installer doesn't know how to handle (wah wah)");
        }
    }
}
