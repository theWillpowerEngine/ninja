using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.common
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

        public NinjaField(string name, string val)
        {
            Name = name.ToLower().Trim();
            _isValue = true;
            Value = val;
        }

        public NinjaField(string name, List<NinjaField> fields)
        {
            Name = name.ToLower().Trim();
            _isValue = false;
            ObjectValue = new NinjaObject(name, fields);
        }

        public NinjaField(string name, NinjaObject ob)
        {
            Name = name.ToLower().Trim();
            _isValue = false;
            ObjectValue = ob;
        }

        public override string ToString()
        {
            if (_isValue)
                return Value;
            return $"<Object {Name}>";
        }
    }
}
