#nullable enable

namespace JsonPropertyAnalyzer.Definitions.SystemTextJson
{
    public class JsonPropertyNameDefinition : IJsonAttribute
    {
        public string Namespace => "System.Text.Json.Serialization";

        public string AttributeName => "JsonPropertyNameAttribute";
        public string AttributeDisplayName => "JsonPropertyName";
        public string? ParameterName => "name";

        public bool HasParameter => true;
    }
}
