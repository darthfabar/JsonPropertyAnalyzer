#nullable enable
using System;
using System.Collections.Generic;
using System.Text;

namespace JsonPropertyAnalyzer.Definitions.SystemTextJson
{
    public class JsonPropertyNameDefinition : IJsonAttribute
    {
        public string Namespace => "System.Text.Json.Serialization";

        public string AttributeName => "JsonPropertyName";

        public string? ParameterName => "name";
    }
}
