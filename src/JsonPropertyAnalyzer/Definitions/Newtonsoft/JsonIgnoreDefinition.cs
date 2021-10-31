#nullable enable

namespace JsonPropertyAnalyzer.Definitions.Newtonsoft
{
    public class JsonIgnoreDefinition : IJsonAttribute
    {
        public string Namespace => "Newtonsoft.Json";

        public string AttributeName => "JsonIgnoreAttribute";

        public string? ParameterName => null;

        public string AttributeDisplayName => "JsonIgnore";

        public bool HasParameter => false;
    }
}
