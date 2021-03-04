using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.common.interop
{
    public class InteropKeyword
    {
        public readonly string Word;
        public readonly int ParamCount;

        public object Handler;

        public InteropKeyword(string word, int count)
        {
            Word = word;
            ParamCount = count;
        }
    }
}
