using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SystemTextJsonIgnoreCodeFix)), Shared]
    public class SystemTextJsonIgnoreCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SystemTextJsonPropertyAnalyzer.IgnoreDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.SystemTextJson.JsonIgnoreDefinition());

        protected override string CodeFixTitle => CodeFixResources.SystemTextJsonIgnoreCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.SystemTextJsonIgnoreCodeFixTitle);
    }
}
