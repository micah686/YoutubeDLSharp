namespace YtdlpPocoGenerator.Models;

public record FieldDefinition
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;

    /// <summary>
    /// Inline type hint parsed from the docstring annotation,
    /// e.g. the "int" in "* width (optional, int) - width in pixels".
    /// </summary>
    public string TypeHint { get; init; } = string.Empty;

    public bool IsOptional { get; init; }
    public List<FieldDefinition> SubFields { get; init; } = [];
}
