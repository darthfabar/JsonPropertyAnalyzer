using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SystemTextJsonPropertyCodeFix)), Shared]
    public class SystemTextJsonPropertyCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SystemTextJsonPropertyAnalyzer.PropertyNameDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.SystemTextJson.JsonPropertyNameDefinition());

        protected override string CodeFixTitle => CodeFixResources.SystemTextJsonPropertyCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.SystemTextJsonPropertyCodeFixTitle);
    }
}
