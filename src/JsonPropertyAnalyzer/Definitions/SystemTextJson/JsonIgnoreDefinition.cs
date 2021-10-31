#nullable enable

namespace JsonPropertyAnalyzer.Definitions.SystemTextJson
{
    public class JsonIgnoreDefinition : IJsonAttribute
    {
        public string Namespace => "System.Text.Json.Serialization";

        public string AttributeName => "JsonIgnoreAttribute";

        public string? ParameterName => null;

        public string AttributeDisplayName => "JsonIgnore";
    }
}
