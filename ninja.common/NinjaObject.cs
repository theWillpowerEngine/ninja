using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ninja.common;

namespace ninja.common
{
    public enum NinjaType
    {
        Value,
        Object
    }

    public class NinjaObject
    {
        protected ConcurrentBag<NinjaField> Fields = new ConcurrentBag<NinjaField>();
        protected string _name;
        
        public NinjaType Type;
        
        public string Name
        {
            get => _name;
        }

        public string Value;

        public List<NinjaField> this[string s]
        {
            get => Fields.Where(f => f.Name == s.ToLower()).ToList();
        }
        public void Add(NinjaField f)
        {
            Fields.Add(f);
        }
        public NinjaField Get(string name, string metaValue)
        {
            return Fields.FirstOrDefault(f => f.Name == name.ToLower().Trim() && f.MetaValue.ToLower().Trim() == metaValue.ToLower().Trim()) ?? NinjaField.Undefined;
        }
        public NinjaField GetValue(string name)
        {
            if (!name.Contains("."))
            {
                var val = this[name];
                if (val.Count == 0)
                    return NinjaField.Undefined;
                
                if(val.Count > 1)
                    return new NinjaField(name, val);

                return val[0];
            }

            var eles = name.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var start = new List<NinjaField>(this[eles[0]]);
            eles.RemoveAt(0);

            foreach (var level in eles) {
                foreach(var field in start)
                {
                    var newStart = field.ObjectValue[level];
                    if (newStart.Count > 0)
                    {
                        start = newStart;
                        break;
                    }
                }
            }

            return start[0];
        }

        public NinjaObject(string name)
        {
            _name = name;
        }
        internal NinjaObject(string name, List<NinjaField> fields)
        {
            _name = name;
            Fields = new ConcurrentBag<NinjaField>(fields);
        }
    }
}
