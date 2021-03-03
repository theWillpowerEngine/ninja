using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang
{
    public class NinjaField
    {
        private static NinjaField _undef = new NinjaField("", (string)null);
        public static NinjaField Undefined => _undef;

        private bool _isValue = false;

        public bool IsUndefined => this == Undefined;
        public bool IsValue
        {
            get => _isValue;
        }

        public readonly string Name;
        public readonly string Value;
        public readonly NinjaObject ObjectValue;
        public string MetaValue;

        public bool HasMetaValue
        {
            get => !string.IsNullOrEmpty(MetaValue);
        }

        internal NinjaField(string name, string val)
        {
            Name = name.ToLower().Trim();
            _isValue = true;
            Value = val;
        }

        internal NinjaField(string name, List<NinjaField> fields)
        {
            Name = name.ToLower().Trim();
            _isValue = false;
            ObjectValue = new NinjaObject(name, fields);
        }

        internal NinjaField(string name, NinjaObject ob)
        {
            Name = name.ToLower().Trim();
            _isValue = false;
            ObjectValue = ob;
        }

        public override string ToString()
        {
            if (_isValue)
                return Value;
            return "<Ninja Object>";
        }
    }
}
