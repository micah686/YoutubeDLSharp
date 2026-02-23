namespace YtDlpSharp.Options;

public class Option<T> : IOption
{
    private T _value = default!;

    public string DefaultOptionString => OptionStrings[0];

    public string[] OptionStrings { get; }

    public bool IsSet { get; private set; }

    public bool IsCustom { get; }

    public T Value
    {
        get => _value;
        set
        {
            IsSet = !EqualityComparer<T>.Default.Equals(value, default!);
            _value = value;
        }
    }

    public Option(params string[] optionStrings)
    {
        OptionStrings = optionStrings;
    }

    public Option(bool isCustom, params string[] optionStrings)
    {
        OptionStrings = optionStrings;
        IsCustom = isCustom;
    }

    public void SetFromString(string s)
    {
        var span = s.AsSpan();
        int firstSpace = span.IndexOf(' ');
        ReadOnlySpan<char> flag = firstSpace >= 0 ? span[..firstSpace] : span;
        ReadOnlySpan<char> stringValue = firstSpace >= 0 ? span[(firstSpace + 1)..].Trim() : ReadOnlySpan<char>.Empty;

        if (typeof(T) != typeof(string) || !stringValue.IsEmpty)
        {
            // Trim quotes for non-StringVals types
            if (stringValue.Length >= 2 && stringValue[0] == '"' && stringValue[^1] == '"')
                stringValue = stringValue[1..^1];
        }

        bool found = false;
        foreach (var opt in OptionStrings)
        {
            if (flag.SequenceEqual(opt.AsSpan()))
            {
                found = true;
                break;
            }
        }
        if (!found)
            throw new ArgumentException("Given string does not match required format.");

        Value = OptionValueFromString<T>(stringValue.ToString());
    }

    public override string ToString()
    {
        if (!IsSet) return string.Empty;
        string val = OptionValueToString(Value);
        return DefaultOptionString + val;
    }

    public IEnumerable<string> ToStringCollection() => [ToString()];

    internal static string OptionValueToString<TVal>(TVal value) => value switch
    {
        bool b => b ? "" : "",
        Enum e => $" {OptionEnumValue(e)}",
        DateTime dt => $" {dt:yyyyMMdd}",
        _ => $" {value}"
    };

    internal static TVal OptionValueFromString<TVal>(string stringValue)
    {
        var type = typeof(TVal);
        var underlying = Nullable.GetUnderlyingType(type);

        if (type == typeof(bool))
            return (TVal)(object)(string.IsNullOrEmpty(stringValue) || bool.Parse(stringValue));
        if (type == typeof(string))
            return (TVal)(object)stringValue;
        if (type == typeof(DateTime))
            return (TVal)(object)DateTime.ParseExact(stringValue, "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
        if (type == typeof(int?) || type == typeof(int))
            return (TVal)(object)int.Parse(stringValue);
        if (type == typeof(long?) || type == typeof(long))
            return (TVal)(object)long.Parse(stringValue);
        if (type == typeof(byte?) || type == typeof(byte))
            return (TVal)(object)byte.Parse(stringValue);
        if (underlying?.IsEnum == true || type.IsEnum)
        {
            var enumType = underlying ?? type;
            foreach (var field in enumType.GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static))
            {
                var name = field.Name;
                if (string.Equals(name, stringValue, StringComparison.OrdinalIgnoreCase))
                    return (TVal)field.GetValue(null)!;
            }
            return (TVal)Enum.Parse(enumType, stringValue, true);
        }
        throw new ArgumentException($"Cannot convert string to type {type}.");
    }

    private static string OptionEnumValue(Enum e)
    {
        var name = e.ToString();
        return name.ToLowerInvariant() switch
        {
            "unspecified" or "none" => "",
            _ => name.ToLowerInvariant()
        };
    }
}
