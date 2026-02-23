namespace YtdlpPocoGenerator.Models;

/// <summary>
/// Represents a single parsed CLI option from yt-dlp's help text.
/// </summary>
public record OptionDefinition
{
    /// <summary>PascalCase property name, e.g. "LimitRate".</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>C# type string, e.g. "long?", "bool", "string".</summary>
    public string CSharpType { get; init; } = string.Empty;

    /// <summary>CLI flag literals, e.g. ["-r", "--limit-rate"].</summary>
    public List<string> Literals { get; init; } = [];

    /// <summary>Raw description lines from the help text.</summary>
    public List<string> DescriptionLines { get; init; } = [];

    /// <summary>Whether this option can be specified multiple times.</summary>
    public bool IsMulti { get; init; }

    /// <summary>Whether this is a deprecated option.</summary>
    public bool IsDeprecated { get; init; }
}

/// <summary>
/// A named section of options, e.g. "General", "Download".
/// </summary>
public record OptionSection
{
    public string Name { get; init; } = string.Empty;
    public List<OptionDefinition> Options { get; init; } = [];
}
