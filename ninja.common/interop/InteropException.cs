using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.common.interop
{
    public class InteropException:ApplicationException
    {
        public InteropException(string msg) : base(msg)
        {

        }
    }
}
