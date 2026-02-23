namespace YtDlpSharp.Options;

public class MultiValue<T>
{
    private readonly List<T> _values;

    public List<T> Values => _values;

    public MultiValue(params T[] values)
    {
        _values = [.. values];
    }

    public static implicit operator MultiValue<T>(T value) => new(value);

    public static implicit operator MultiValue<T>(T[] values) => new(values);

    public static explicit operator T(MultiValue<T> value) =>
        value.Values.Count == 1
            ? value.Values[0]
            : throw new InvalidCastException($"Cannot cast sequence of values to {typeof(T)}.");

    public static explicit operator T[](MultiValue<T> value) => [.. value.Values];
}
