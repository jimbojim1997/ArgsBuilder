using ArgumentBuilder.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ArgumentBuilder
{
    public static class ArgsBuilder
    {
        public static T Build<T>(string[] args) where T : new()
        {
            T data = new T();
            var keyValues = new Dictionary<string, List<string>>();
            var orderedArgs = new List<string>();

            // Get data
            foreach(var property in data.GetType().GetRuntimeProperties())
            {
                var kvpAttributes = new List<ArgsNamedAttribute>();

                foreach(var attribute in property.GetCustomAttributes())
                {
                    if (attribute.GetType() == typeof(ArgsNamedAttribute)) kvpAttributes.Add(attribute as ArgsNamedAttribute);
                    else if (attribute.GetType() == typeof(DefaultValueAttribute)) SetValue<T>(data, property, (attribute as DefaultValueAttribute)?.Value.ToString());
                }

                foreach(var attribute in kvpAttributes)
                {
                    for(int i = 0; i < args.Length; i++)
                    {
                        string arg = args[i];
                        if (arg == attribute.Name)
                        {
                            if(attribute.ValueParseMethod == ArgsValueParseMethod.Boolean)
                            {
                                if (!keyValues.ContainsKey(arg))
                                {
                                    keyValues.Add(arg, new List<string>());
                                    args[i] = null;
                                }
                            }else if(attribute.ValueParseMethod == ArgsValueParseMethod.Space && i <= args.Length - 2)
                            {
                                if (keyValues.ContainsKey(arg))
                                {
                                    keyValues[arg].Add(args[i + 1]);
                                    args[i] = null;
                                    args[i + 1] = null;
                                }
                                else
                                {
                                    keyValues.Add(arg, new List<string>());
                                    keyValues[arg].Add(args[i + 1]);
                                    args[i] = null;
                                    args[i + 1] = null;
                                }
                            }
                        }
                    }
                }
            }

            foreach (string arg in args) if (arg != null) orderedArgs.Add(arg);

            //set data
            foreach (var property in data.GetType().GetRuntimeProperties())
            {
                foreach (var attribute in property.GetCustomAttributes())
                {
                    if (attribute.GetType() == typeof(ArgsNamedAttribute))
                    {
                        var attr = attribute as ArgsNamedAttribute;
                        foreach(var pair in keyValues)
                        {
                            if(pair.Key == attr.Name) SetValue<T>(data, property, pair.Value);
                        }
                    }
                    else if (attribute.GetType() == typeof(ArgsOrderedAttribute))
                    {
                        var attr = attribute as ArgsOrderedAttribute;
                        if (attr.Index < orderedArgs.Count) SetValue<T>(data, property, orderedArgs[attr.Index]);
                    }
                }
            }

            return data;
        }

        private static void SetValue<T>(T data, PropertyInfo property, string value)
        {
            SetValue<T>(data, property, new List<string>(new String[] { value }));
        }

        private static void SetValue<T>(T data, PropertyInfo property, List<string> value)
        {
            if (value == null) return;

            Type type = property.PropertyType;

            if (type == typeof(string)) property.SetValue(data, value[0]);
            else if (type == typeof(bool)) property.SetValue(data, true);
            else if (type == typeof(sbyte)) property.SetValue(data, Convert.ToSByte(value[0]));
            else if (type == typeof(byte)) property.SetValue(data, Convert.ToByte(value[0]));
            else if (type == typeof(char)) property.SetValue(data, Convert.ToChar(value[0]));
            else if (type == typeof(Int16)) property.SetValue(data, Convert.ToInt16(value[0]));
            else if (type == typeof(Int32)) property.SetValue(data, Convert.ToInt32(value[0]));
            else if (type == typeof(Int64)) property.SetValue(data, Convert.ToInt64(value[0]));
            else if (type == typeof(UInt16)) property.SetValue(data, Convert.ToUInt16(value[0]));
            else if (type == typeof(UInt32)) property.SetValue(data, Convert.ToUInt32(value[0]));
            else if (type == typeof(UInt64)) property.SetValue(data, Convert.ToUInt64(value[0]));
            else if (type == typeof(float)) property.SetValue(data, Convert.ToSingle(value[0]));
            else if (type == typeof(double)) property.SetValue(data, Convert.ToDouble(value[0]));
            else if (type == typeof(Decimal)) property.SetValue(data, Convert.ToDecimal(value[0]));
            else if (type == typeof(DateTime)) property.SetValue(data, Convert.ToDateTime(value[0]));
            else if (type == typeof(IEnumerable<string>)) property.SetValue(data, value as IEnumerable<string>);
        }

        private static bool DoesTypeInherit(Type type, Type inheritFrom)
        {
            foreach (Type t in type.GetInterfaces()) if (t.Equals(inheritFrom)) return true;
            return false;
        }
    }
}
