#nullable enable

namespace JsonPropertyAnalyzer.Definitions.Newtonsoft
{
    public class JsonPropertyDefinition : IJsonAttribute
    {
        public string Namespace => "Newtonsoft.Json";

        public string AttributeName => "JsonPropertyAttribute";

        public string? ParameterName => "propertyName";

        public string AttributeDisplayName => "JsonProperty";
        public bool HasParameter => true;
    }
}
