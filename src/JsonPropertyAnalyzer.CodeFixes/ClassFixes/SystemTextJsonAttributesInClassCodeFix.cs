#nullable enable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SystemTextJsonAttributesInClassCodeFix)), Shared]
    public class SystemTextJsonAttributesInClassCodeFix : JsonAttributesInClassCodeFixAbstract
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SystemTextJsonPropertyAnalyzer.ClassWithPropertiesDiagnosticId); }
        }

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonIgnorePropertiesHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.SystemTextJson.JsonIgnoreDefinition());

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonPropertyNamePropertiesHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.SystemTextJson.JsonPropertyNameDefinition());

        protected override string CodeFixTitle => CodeFixResources.SystemTextJsonClassCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.SystemTextJsonClassCodeFixTitle);
    }
}
