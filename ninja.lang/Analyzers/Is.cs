using ninja.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang
{
    public static class Is
    {
        public static bool MetaedObject(NinjaObject o)
        {
            return o.Type == NinjaType.Object && !string.IsNullOrEmpty(o.Value);
        }

        public static bool MetaedObject(NinjaObject o, string mv)
        {
            return o.Type == NinjaType.Object && o.Value?.ToLower() == mv.ToLower();
        }
    }
}
