namespace YtdlpPocoGenerator.Models;

public record FieldDefinition
{
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsOptional { get; init; }
    public List<FieldDefinition> SubFields { get; init; } = [];
}
