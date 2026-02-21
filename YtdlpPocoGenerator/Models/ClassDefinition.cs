namespace YtdlpPocoGenerator.Models;

public record ClassDefinition
{
    public string ClassName { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
    public List<PropertyDefinition> Properties { get; init; } = [];
}

public record PropertyDefinition
{
    public string JsonName { get; init; } = string.Empty;
    public string CSharpName { get; init; } = string.Empty;
    public string CSharpType { get; init; } = string.Empty;
    public string Summary { get; init; } = string.Empty;
}
