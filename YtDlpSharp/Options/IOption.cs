namespace YtDlpSharp.Options;

public interface IOption
{
    string DefaultOptionString { get; }
    string[] OptionStrings { get; }
    bool IsSet { get; }
    bool IsCustom { get; }
    void SetFromString(string s);
    IEnumerable<string> ToStringCollection();
}
