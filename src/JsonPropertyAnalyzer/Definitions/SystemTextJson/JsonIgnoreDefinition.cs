#nullable enable

namespace JsonPropertyAnalyzer.Definitions.SystemTextJson
{
    public class JsonIgnoreDefinition : IJsonAttribute
    {
        public string Namespace => "System.Text.Json.Serialization";

        public string AttributeName => "JsonIgnore";

        public string? ParameterName => null;
    }
}
