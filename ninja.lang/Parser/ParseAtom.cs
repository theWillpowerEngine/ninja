using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang
{
    internal enum ParseAtomType
    {
        CodePrefix,
        ObjectPrefix,
        NameValue,
        NameValueCodePrefix,
        NameValueObjectPrefix,
        NameValueValue
    }
    
    internal class ParseAtom
    {
        internal string Name;
        internal string Value;
        internal string Value2;

        internal bool HasSecondValue
        {
            get => !string.IsNullOrEmpty(Value2);
        }

        internal ParseAtomType AtomType;
    }
}
