#nullable enable

namespace JsonPropertyAnalyzer.Definitions
{
    public interface IJsonAttribute
    {
        string Namespace { get; }
        string AttributeName { get; }
        string AttributeDisplayName { get; }
        string? ParameterName { get; }

        bool HasParameter { get; }
    }
}
