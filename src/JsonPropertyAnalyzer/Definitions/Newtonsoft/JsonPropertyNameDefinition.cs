#nullable enable

namespace JsonPropertyAnalyzer.Definitions.Newtonsoft
{
    public class JsonPropertDefinition : IJsonAttribute
    {
        public string Namespace => "Newtonsoft.Json";

        public string AttributeName => "JsonProperty";

        public string? ParameterName => "propertyName";
    }
}
