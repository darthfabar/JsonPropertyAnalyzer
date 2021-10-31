#nullable enable

namespace JsonPropertyAnalyzer.Definitions.Newtonsoft
{
    public class JsonIgnoreDefinition : IJsonAttribute
    {
        public string Namespace => "Newtonsoft.Json";

        public string AttributeName => "JsonIgnore";

        public string? ParameterName => null;
    }
}
