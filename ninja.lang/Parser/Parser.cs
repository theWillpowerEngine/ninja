using ninja.common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ninja.lang
{
    internal static class Parser
    {
        private static void Error(string error)
        {
            throw new Exception(error);
        }
        
        private static int CalcIndent(string line)
        {
            var ret = 0;

            for(var i=0; i<line.Length; i++)
            {
                switch(line[i])
                {
                    case ' ':
                    case '\t':
                        ret += 1;
                        continue;

                    default:
                        return ret;
                }
            }

            return ret;
        }

        private static ParseAtom ParseLine(string line)
        {
            if (line.Trim().EndsWith("{"))
            {
                if(line.Contains(":"))
                {
                    var eles = line.Trim().TrimEnd('{').Split(":");
                    if (eles.Length != 2)
                        Error("Invalid line composition: \n\t" + line);

                    return new ParseAtom()
                    {
                        AtomType = ParseAtomType.NameValueCodePrefix,
                        Name = eles[0].Trim().ToLower(),
                        Value2 = eles[1].Trim()
                    };
                }
                else
                {
                    return new ParseAtom()
                    {
                        AtomType = ParseAtomType.CodePrefix,
                        Name = line.Trim().TrimEnd('{').Trim().ToLower()
                    };
                }
            } 
            else if (line.Trim().EndsWith(":"))
            {
                if (line.Substring(0, line.Length-1).Contains(":"))
                {
                    var eles = line.Trim().TrimEnd(':').Split(":");
                    return new ParseAtom()
                    {
                        AtomType = ParseAtomType.NameValueObjectPrefix,
                        Name = eles[0].Trim().ToLower(),
                        Value2 = eles[1].Trim()
                    };
                }
                else
                {
                    return new ParseAtom()
                    {
                        AtomType = ParseAtomType.ObjectPrefix,
                        Name = line.Trim().TrimEnd(':').Trim().ToLower()
                    };
                }
            }
            else if (line.Contains(":"))
            {
                var eles = line.Split(":");
                if (eles.Length == 2)
                {
                    return new ParseAtom()
                    {
                        AtomType = ParseAtomType.NameValue,
                        Name = eles[0].Trim().ToLower(),
                        Value = eles[1].Trim()
                    };
                } 
                else if (eles.Length == 3)
                {
                    return new ParseAtom()
                    {
                        AtomType = ParseAtomType.NameValueValue,
                        Name = eles[0].Trim().ToLower(),
                        Value = eles[2].Trim(),
                        Value2 = eles[1].Trim()
                    };
                } else 
                    Error("Invalid line composition: \n\t" + line);

                
            }
            else
                Error("Can't handle line: \n\t" + line);

            return null;
        }

        private static NinjaObject Scan(string code)
        {
            int n = 0;
            return Scan(code, ref n);
        }

        private static NinjaObject Scan(string code, ref int startLine, int baseIndent=-1)
        {
            var lines = code.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            NinjaObject retVal = null;
            bool scanningCode = false;
            bool first = true;

            for(; startLine<lines.Length; startLine++)
            {
                var line = lines[startLine];

                if (string.IsNullOrWhiteSpace(line))
                    continue;

                //Declaration Line
                if(first)
                {
                    first = false;
                    if (baseIndent == -1)
                        baseIndent = CalcIndent(line);

                    var atom = ParseLine(line);
                    retVal = new NinjaObject(atom.Name);

                    switch (atom.AtomType)
                    {
                        case ParseAtomType.CodePrefix:
                            retVal.Type = NinjaType.Value;
                            scanningCode = true;
                            break;

                        case ParseAtomType.NameValueCodePrefix:
                            if (startLine == 0)
                                retVal.Value = atom.Value2;
                            retVal.Type = NinjaType.Value;
                            scanningCode = true;
                            break;

                        case ParseAtomType.ObjectPrefix:
                            retVal.Type = NinjaType.Object;
                            break;

                        case ParseAtomType.NameValueObjectPrefix:
                            if (startLine == 0)
                                retVal.Value = atom.Value2;
                            retVal.Type = NinjaType.Object;
                            break;

                        case ParseAtomType.NameValue:
                        case ParseAtomType.NameValueValue:
                            Error("Cannot have an name/value pair on the top level");
                            break;

                        default:
                            Error("Unknown top level element, " + atom.Name);
                            break;
                    }
                } 
                else  //The other lines
                {
                    if(CalcIndent(line) == baseIndent)
                    {
                        if (scanningCode)
                        {
                            if (line.Trim() != "}")
                                Error("Expected '}' to end object, not: " + line);
                        } 
                        else
                        {
                            startLine -= 1;
                        }

                        return retVal;
                    } 
                    else
                    {
                        if(scanningCode)
                            retVal.Value += line;
                        else
                        {
                            var lineAtom = ParseLine(line);
                            switch(lineAtom.AtomType)
                            {
                                case ParseAtomType.NameValue:
                                    retVal.Add(new NinjaField(lineAtom.Name, lineAtom.Value));
                                    break;

                                case ParseAtomType.NameValueValue:
                                    if (!lineAtom.HasSecondValue)
                                        Error("Name/Value Values must contain values, offending line: \n\t" + line);
                                    retVal.Add(new NinjaField(lineAtom.Name, lineAtom.Value)
                                    {
                                        MetaValue = lineAtom.Value2
                                    });
                                    break;

                                case ParseAtomType.CodePrefix:
                                    retVal.Add(new NinjaField(lineAtom.Name, Scan(code, ref startLine, CalcIndent(line))));
                                    break;

                                case ParseAtomType.NameValueCodePrefix:
                                    if (!lineAtom.HasSecondValue)
                                        Error("Name/Value code prefixes must contain values, offending line: \n\t" + line);
                                    
                                    var field = new NinjaField(lineAtom.Name, Scan(code, ref startLine, CalcIndent(line)));
                                    field.MetaValue = lineAtom.Value2;
                                    retVal.Add(field);
                                    break;

                                case ParseAtomType.ObjectPrefix:
                                    retVal.Add(new NinjaField(lineAtom.Name, Scan(code, ref startLine, CalcIndent(line))));
                                    break;

                                case ParseAtomType.NameValueObjectPrefix:
                                    if (!lineAtom.HasSecondValue)
                                        Error("Name/Value object prefixes must contain values, offending line: \n\t" + line);

                                    var field2 = new NinjaField(lineAtom.Name, Scan(code, ref startLine, CalcIndent(line)));
                                    field2.MetaValue = lineAtom.Value2;
                                    retVal.Add(field2);
                                    break;

                                default:
                                    Error("New ParseAtomType not handled in line parser:  " + lineAtom.AtomType);
                                    break;
                            }
                        }
                    }
                }
            }

            if (scanningCode)
                Error("Expected '}' to end object, not end-of-code");

            return retVal;
        }

        internal static NinjaObject ToObject(string code)
        {
            return Scan(code);
        }
    }
}
