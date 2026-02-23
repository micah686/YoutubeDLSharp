namespace YtDlpSharp.Options;

public class MultiOption<T> : IOption
{
    private MultiValue<T> _value = default!;

    public string DefaultOptionString => OptionStrings[^1];

    public string[] OptionStrings { get; }

    public bool IsSet { get; private set; }

    public bool IsCustom { get; }

    public MultiValue<T> Value
    {
        get => _value;
        set
        {
            IsSet = value is not null;
            _value = value!;
        }
    }

    public MultiOption(params string[] optionStrings)
    {
        OptionStrings = optionStrings;
    }

    public MultiOption(bool isCustom, params string[] optionStrings)
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

        if (stringValue.Length >= 2 && stringValue[0] == '"' && stringValue[^1] == '"')
            stringValue = stringValue[1..^1];

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

        T newValue = Option<T>.OptionValueFromString<T>(stringValue.ToString());
        if (!IsSet)
            Value = newValue;
        else
            Value.Values.Add(newValue);
    }

    public override string ToString() => string.Join(" ", ToStringCollection());

    public IEnumerable<string> ToStringCollection()
    {
        if (!IsSet) return [""];
        return Value.Values.Select(v => DefaultOptionString + Option<T>.OptionValueToString(v));
    }
}
