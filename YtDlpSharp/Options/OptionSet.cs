using System.Reflection;

namespace YtDlpSharp.Options;

public partial class OptionSet : ICloneable
{
    private sealed class OptionComparer : IEqualityComparer<IOption>
    {
        public bool Equals(IOption? x, IOption? y) =>
            x?.DefaultOptionString == y?.DefaultOptionString;
        public int GetHashCode(IOption obj) =>
            obj.DefaultOptionString.GetHashCode();
    }

    private static readonly OptionComparer Comparer = new();

    public static readonly OptionSet Default = new();

    public void WriteConfigFile(string path)
    {
        File.WriteAllLines(path, GetOptionFlags());
    }

    public override string ToString() => " " + string.Join(" ", GetOptionFlags());

    public IEnumerable<string> GetOptionFlags() =>
        GetKnownOptions()
            .Concat(CustomOptions)
            .SelectMany(opt => opt.ToStringCollection())
            .Where(value => !string.IsNullOrWhiteSpace(value));

    internal IEnumerable<IOption> GetKnownOptions() =>
        GetType()
            .GetRuntimeFields()
            .Where(p => p.FieldType.IsGenericType && p.FieldType.GetInterfaces().Contains(typeof(IOption)))
            .Select(p => p.GetValue(this))
            .Cast<IOption>();

    public OptionSet OverrideOptions(OptionSet overrideOptions, bool forceOverride = false)
    {
        var cloned = (OptionSet)Clone();
        cloned.CustomOptions = cloned.CustomOptions
            .Concat(overrideOptions.CustomOptions)
            .Distinct(Comparer)
            .ToArray();

        var overrideFields = overrideOptions.GetType().GetRuntimeFields()
            .Where(p => p.FieldType.IsGenericType && p.FieldType.GetInterfaces().Contains(typeof(IOption)));

        foreach (var field in overrideFields)
        {
            var fieldValue = (IOption)field.GetValue(overrideOptions)!;
            if (forceOverride || fieldValue.IsSet)
            {
                cloned.GetType()
                    .GetField(field.Name, BindingFlags.NonPublic | BindingFlags.Instance)
                    ?.SetValue(cloned, fieldValue);
            }
        }

        return cloned;
    }

    public static OptionSet FromString(IEnumerable<string> lines)
    {
        var optSet = new OptionSet();
        var customOptions = GetOptions(lines, optSet.GetKnownOptions())
            .Where(option => option.IsCustom)
            .ToArray();
        optSet.CustomOptions = customOptions;
        return optSet;
    }

    private static IEnumerable<IOption> GetOptions(IEnumerable<string> lines, IEnumerable<IOption> options)
    {
        var knownOptions = options.ToList();

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line.StartsWith('#') || string.IsNullOrWhiteSpace(line))
                continue;

            string[] segments = line.Split(' ');
            string flag = segments[0];

            IOption? knownOption = knownOptions.FirstOrDefault(o => o.OptionStrings.Contains(flag));
            IOption customOption = segments.Length > 1
                ? new Option<string>(isCustom: true, flag)
                : new Option<bool>(isCustom: true, flag);

            var option = knownOption ?? customOption;
            option.SetFromString(line);
            yield return option;
        }
    }

    public static OptionSet LoadConfigFile(string path) => FromString(File.ReadAllLines(path));

    public object Clone() => FromString(GetOptionFlags());
}
