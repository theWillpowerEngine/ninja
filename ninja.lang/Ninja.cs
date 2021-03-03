using System;

namespace ninja.lang
{
    public class Ninja
    {
        public static NinjaContext Parse(string code)
        {
            return new NinjaContext(Parser.ToObject(code));
        }
    }
}
