namespace YtdlpPocoGenerator.Models;

public record EnumDefinition
{
    /// <summary>C# enum type name, e.g. "LiveStatus".</summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>Original snake_case field name, e.g. "live_status".</summary>
    public string FieldName { get; init; } = string.Empty;

    /// <summary>Short description to use as the XML summary on the enum.</summary>
    public string Summary { get; init; } = string.Empty;

    /// <summary>
    /// Each entry is (jsonValue, csharpMemberName), e.g. ("is_live", "IsLive").
    /// </summary>
    public List<(string JsonValue, string CSharpName)> Values { get; init; } = [];
}
