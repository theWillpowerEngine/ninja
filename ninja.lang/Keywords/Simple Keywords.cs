using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang.keywords
{
    internal class PrintKeyword : Keyword
    {
        internal override void Execute(List<string> parms)
        {
            Console.WriteLine(parms[0]);
        }

        internal PrintKeyword() : base(1)
        {

        }
    }
}
