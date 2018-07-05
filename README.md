# Argument Builder
Argument Builder is a .NET Standard library for populating an object's properties with values from an argument string array.

## Usage
Create a class with a default (parameterless) constructor and add one of the below attributes to each property.

* `[ArgumentBuilder.Attributes.ArgsNamedAttribute(string name, ArgsValueParseMethod valueParseMethod)]`  
  This attribute is used when an argument has a name/key-value pair or as a flag, the `ArgumentBuilder.Attributes.ArgsValueParseMethod` is used to describe for format of the argument, for example:
    * `ArgsValueParseMethod.Space` : `--name "John Doe"`
    * `ArgsValueParseMethod.Colon` : `"--name:John Doe"`
    * `ArgsValueParseMethod.Equals` : `"--name=John Doe"`
    * `ArgsValueParseMethod.Boolean` : `--alive`
  
  The format of the name/key doesn't matter. When a value contains a space character it should be surrounded with quotes (") otherwise only the first part will be parsed.

* `[ArgumentBuilder.Attributes.ArgsOrderedAttribute(int index)]`  
  This attribute is used when an argument has a specific order in the attributes list. Named arguments are ignored when parsing the ordered arguments. Note: a named argument is only ignored if a property has an `ArgsNamedAttribute` with that argument name, otherwise it is parsed as an ordered argument. The `index` is 0 based.

* `[System.DefaultValueAttribute(object value)]`  
  This attribute is used to set a default value for the give property. It can be used on any property and is required to be used alongside `ArgumentBuilder.Attributes.ArgsValueParseMethod.BooleanInverted` for that attribute to be useful.

### Examples
The below is an example on an argument string and matching class structure.  
`--name "John Doe" --age 20 --alive --hobby programming -h flying -h gaming`

```csharp
class Options
{
    [ArgsNamed("--name", ArgsValueParseMethod.Space)]
    public String Name { get; set; }

    [ArgsNamed("--age", ArgsValueParseMethod.Space)]
    public int Age { get; set; }

    [ArgsNamed("--alive", ArgsValueParseMethod.Boolean)]
    public bool IsAlive { get; set; }

    [ArgsNamed("--hobby", ArgsValueParseMethod.Space)]
    [ArgsNamed("-h", ArgsValueParseMethod.Space)]
    public IEnumerable<string> Hobbies { get; set; }

    public Options() { }
}
```

## Supported Property Types
The following property types are supported, if a type is used that isn't in this list an `InvalidTypeException` is thrown.
* `string`
* `bool`
* `sbyte`
* `byte`
* `char`
* `Int16`
* `Int32`
* `Int64`
* `UInt16`
* `UInt32`
* `UInt64`
* `float`
* `double`
* `Decimal`
* `DateTime`
* `IEnumberble<string>`
* `IEnumberble<bool>`
* `IEnumberble<sbyte>`
* `IEnumberble<byte>`
* `IEnumberble<char>`
* `IEnumberble<Int16>`
* `IEnumberble<Int32>`
* `IEnumberble<Int64>`
* `IEnumberble<UInt16>`
* `IEnumberble<UInt32>`
* `IEnumberble<UInt64>`
* `IEnumberble<float>`
* `IEnumberble<double>`
* `IEnumberble<Decimal>`
* `IEnumberble<DateTime>`
