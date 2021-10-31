#nullable enable

namespace JsonPropertyAnalyzer.Definitions
{
    internal interface IJsonAttribute
    {
        string Namespace { get; }
        string AttributeName { get; }
        string? ParameterName { get; }
    }
}
