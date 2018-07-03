using ArgumentBuilder.Attributes;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace ArgumentBuilder
{
    public static class ArgsBuilder
    {
        /// <summary>
        /// Populates the properties on the type <c>T</c> to the value supplied in <c>args</c>.
        /// </summary>
        /// <typeparam name="T">The type to create and populate with values.</typeparam>
        /// <param name="args">The argument array to read the values from.</param>
        /// <returns>An object of type <c>T</c> with the properties populated with the values from <c>args</c>.</returns>
        /// <exception cref="InvalidTypeException">Thrown when <c>T</c> has a property of a type that isn't supported.</exception>
        public static T Build<T>(string[] args) where T : new()
        {
            string[] workingArgs = new string[args.Length];
            for (int i = 0; i < args.Length; i++) workingArgs[i] = args[i];

            T data = new T();

            var keyValues = new Dictionary<string, List<string>>();
            var orderedArgs = new List<string>();

            // Get data
            foreach(var property in data.GetType().GetRuntimeProperties())
            {
                if (!IsValidType(property.PropertyType)) throw new InvalidTypeException($"Type \"{property.PropertyType.FullName}\" is not a supported type. See documentation for supported types.");

                var kvpAttributes = new List<ArgsNamedAttribute>();

                foreach(var attribute in property.GetCustomAttributes())
                {
                    if (attribute.GetType() == typeof(ArgsNamedAttribute)) kvpAttributes.Add(attribute as ArgsNamedAttribute);
                    else if (attribute.GetType() == typeof(DefaultValueAttribute)) SetValue<T>(data, property, (attribute as DefaultValueAttribute)?.Value.ToString());
                }

                foreach(var attribute in kvpAttributes)
                {
                    for(int i = 0; i < workingArgs.Length; i++)
                    {
                        string arg = workingArgs[i];
                        if (arg == null) continue;
                        if (arg == attribute.Name)
                        {
                            if (attribute.ValueParseMethod == ArgsValueParseMethod.Boolean)
                            {
                                if (!keyValues.ContainsKey(arg)) keyValues.Add(arg, new List<string>());
                                keyValues[arg].Add("True");
                                workingArgs[i] = null;
                            }
                            else if (attribute.ValueParseMethod == ArgsValueParseMethod.BooleanInverted)
                            {
                                if (!keyValues.ContainsKey(arg)) keyValues.Add(arg, new List<string>());
                                keyValues[arg].Add("False");
                                workingArgs[i] = null;
                            }
                            else if (attribute.ValueParseMethod == ArgsValueParseMethod.Space && i <= workingArgs.Length - 2)
                            {
                                if (!keyValues.ContainsKey(arg)) keyValues.Add(arg, new List<string>());
                                keyValues[arg].Add(workingArgs[i + 1]);
                                workingArgs[i] = null;
                                workingArgs[i + 1] = null;
                            }
                        }else if (arg.Contains(attribute.Name))
                        {
                            if (attribute.ValueParseMethod == ArgsValueParseMethod.Colon)
                            {
                                KeyValuePair<string, string> pair = SplitStringKeyValuePair(arg, ':');
                                if (pair.Key == attribute.Name)
                                {
                                    if(!keyValues.ContainsKey(pair.Key)) keyValues.Add(pair.Key, new List<string>());
                                    keyValues[pair.Key].Add(pair.Value);
                                }
                                
                            }
                            else if (attribute.ValueParseMethod == ArgsValueParseMethod.Equals)
                            {
                                KeyValuePair<string, string> pair = SplitStringKeyValuePair(arg, '=');
                                if (pair.Key == attribute.Name)
                                {
                                    if (!keyValues.ContainsKey(pair.Key)) keyValues.Add(pair.Key, new List<string>());
                                    keyValues[pair.Key].Add(pair.Value);
                                }
                            }
                        }
                    }
                }
            }

            foreach (string arg in workingArgs) if (arg != null) orderedArgs.Add(arg);

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

        /// <summary>
        /// Sets the <c>property</c> value to <c>value</c> on the <c>data</c> object.
        /// </summary>
        /// <typeparam name="T">The type of property to be set on.</typeparam>
        /// <param name="data">The data to set the value on.</param>
        /// <param name="property">The property to be set.</param>
        /// <param name="value">The value to set the property to.</param>
        private static void SetValue<T>(T data, PropertyInfo property, string value)
        {
            SetValue<T>(data, property, new List<string>(new String[] { value }));
        }

        /// <summary>
        /// Sets the <c>property</c> value to <c>value</c> on the <c>data</c> object.
        /// </summary>
        /// <typeparam name="T">The type of property to be set on.</typeparam>
        /// <param name="data">The data to set the value on.</param>
        /// <param name="property">The property to be set.</param>
        /// <param name="values">The value to set the property to.</param>
        /// <exception cref="InvalidTypeException">Thrown when <c>T</c> is a type that isn't supported.</exception>
        private static void SetValue<T>(T data, PropertyInfo property, IList<string> values)
        {
            if (values == null || values.Count == 0) return;

            Type type = property.PropertyType;

            if (type == typeof(string)) property.SetValue(data, values[0]);
            else if (type == typeof(bool)) property.SetValue(data, Convert.ToBoolean(values[0]));
            else if (type == typeof(sbyte)) property.SetValue(data, Convert.ToSByte(values[0]));
            else if (type == typeof(byte)) property.SetValue(data, Convert.ToByte(values[0]));
            else if (type == typeof(char)) property.SetValue(data, Convert.ToChar(values[0]));
            else if (type == typeof(Int16)) property.SetValue(data, Convert.ToInt16(values[0]));
            else if (type == typeof(Int32)) property.SetValue(data, Convert.ToInt32(values[0]));
            else if (type == typeof(Int64)) property.SetValue(data, Convert.ToInt64(values[0]));
            else if (type == typeof(UInt16)) property.SetValue(data, Convert.ToUInt16(values[0]));
            else if (type == typeof(UInt32)) property.SetValue(data, Convert.ToUInt32(values[0]));
            else if (type == typeof(UInt64)) property.SetValue(data, Convert.ToUInt64(values[0]));
            else if (type == typeof(float)) property.SetValue(data, Convert.ToSingle(values[0]));
            else if (type == typeof(double)) property.SetValue(data, Convert.ToDouble(values[0]));
            else if (type == typeof(Decimal)) property.SetValue(data, Convert.ToDecimal(values[0]));
            else if (type == typeof(DateTime)) property.SetValue(data, Convert.ToDateTime(values[0]));
            else if (type == typeof(IEnumerable<string>)) property.SetValue(data, values);
            else if (type == typeof(IEnumerable<bool>)) property.SetValue(data, ConvertList(values, v => Convert.ToBoolean(v)));
            else if (type == typeof(IEnumerable<sbyte>)) property.SetValue(data, ConvertList(values, v => Convert.ToSByte(v)));
            else if (type == typeof(IEnumerable<byte>)) property.SetValue(data, ConvertList(values, v => Convert.ToByte(v)));
            else if (type == typeof(IEnumerable<char>)) property.SetValue(data, ConvertList(values, v => Convert.ToChar(v)));
            else if (type == typeof(IEnumerable<Int16>)) property.SetValue(data, ConvertList(values, v => Convert.ToInt16(v)));
            else if (type == typeof(IEnumerable<Int32>)) property.SetValue(data, ConvertList(values, v => Convert.ToInt32(v)));
            else if (type == typeof(IEnumerable<Int64>)) property.SetValue(data, ConvertList(values, v => Convert.ToInt64(v)));
            else if (type == typeof(IEnumerable<UInt16>)) property.SetValue(data, ConvertList(values, v => Convert.ToUInt16(v)));
            else if (type == typeof(IEnumerable<UInt32>)) property.SetValue(data, ConvertList(values, v => Convert.ToUInt32(v)));
            else if (type == typeof(IEnumerable<UInt64>)) property.SetValue(data, ConvertList(values, v => Convert.ToUInt64(v)));
            else if (type == typeof(IEnumerable<float>)) property.SetValue(data, ConvertList(values, v => Convert.ToSingle(v)));
            else if (type == typeof(IEnumerable<double>)) property.SetValue(data, ConvertList(values, v => Convert.ToDouble(v)));
            else if (type == typeof(IEnumerable<Decimal>)) property.SetValue(data, ConvertList(values, v => Convert.ToDecimal(v)));
            else if (type == typeof(IEnumerable<DateTime>)) property.SetValue(data, ConvertList(values, v => Convert.ToDateTime(v)));
            else throw new InvalidTypeException($"Type \"{typeof(T).FullName}\" is not a supported type. See documentation for supported types.");
        }

        /// <summary>
        /// Checks if the type is supported by the builder.
        /// </summary>
        /// <param name="type">The type to check support for.</param>
        /// <returns>True if it's a supported type, false if not.</returns>
        private static bool IsValidType(Type type)
        {
            if (type == typeof(string) ||
                type == typeof(bool) ||
                type == typeof(sbyte) ||
                type == typeof(byte) ||
                type == typeof(char) ||
                type == typeof(Int16) ||
                type == typeof(Int32) ||
                type == typeof(Int64) ||
                type == typeof(UInt16) ||
                type == typeof(UInt32) ||
                type == typeof(UInt64) ||
                type == typeof(float) ||
                type == typeof(double) ||
                type == typeof(Decimal) ||
                type == typeof(DateTime) ||
                type == typeof(IEnumerable<string>) ||
                type == typeof(IEnumerable<bool>) ||
                type == typeof(IEnumerable<sbyte>) ||
                type == typeof(IEnumerable<char>) ||
                type == typeof(IEnumerable<Int16>) ||
                type == typeof(IEnumerable<Int32>) ||
                type == typeof(IEnumerable<Int64>) ||
                type == typeof(IEnumerable<UInt16>) ||
                type == typeof(IEnumerable<UInt32>) ||
                type == typeof(IEnumerable<UInt64>) ||
                type == typeof(IEnumerable<float>) ||
                type == typeof(IEnumerable<double>) ||
                type == typeof(IEnumerable<Decimal>) ||
                type == typeof(IEnumerable<DateTime>)
                ) return true;
            return false;
        }

        /// <summary>
        /// Converts and <c><![CDATA[IEnumberable<string>]]></c> to an <c><![CDATA[IEnumberable<T>]]>]]></c>.
        /// </summary>
        /// <typeparam name="T">The type to convert items to.</typeparam>
        /// <param name="values">The values to be converted.</param>
        /// <param name="convert">Function that performs the conversion.</param>
        /// <returns>A converted <c><![CDATA[IEnumberable<T>]]>]]></c> from <c>values</c>.</returns>
        private static IEnumerable<T> ConvertList<T>(IEnumerable<string> values, Func<string, T> convert)
        {
            List<T> result = new List<T>();
            foreach (string v in values) result.Add(convert(v));
            return result;
        }

        /// <summary>
        /// Splits a string into a <c><![CDATA[KeyValuesPair<string, string>]]></c> at the specified character.
        /// </summary>
        /// <param name="text">The text to be split.</param>
        /// <param name="splitAt">The character to split at.</param>
        /// <returns>A <c><![CDATA[KeyValuesPair<string, string>]]></c> from the supplied text split at <c>splitAt</c>.</returns>
        private static KeyValuePair<string, string> SplitStringKeyValuePair(string text, char splitAt)
        {
            int splitAtIndex = text.IndexOf(splitAt);
            if (splitAtIndex == -1) return default(KeyValuePair<string, string>);
            string key = text.Substring(0, splitAtIndex);
            string value = text.Substring(splitAtIndex + 1, text.Length - splitAtIndex - 1);

            return new KeyValuePair<string, string>(key, value);
        }

        /// <summary>
        /// Checks if a type inherits from another.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <param name="inheritFrom">The type to check if <c>type</c> inherits from.</param>
        /// <returns>True if <c>type</c> inherits from <c>inheritsFrom</c>, False if not.</returns>
        private static bool DoesTypeInherit(Type type, Type inheritFrom)
        {
            foreach (Type t in type.GetInterfaces()) if (t.Equals(inheritFrom)) return true;
            return false;
        }
    }
}
