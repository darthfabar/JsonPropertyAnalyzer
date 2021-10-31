using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NewtonsoftJsonIgnoreCodeFix)), Shared]
    public class NewtonsoftJsonIgnoreCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NewtonsoftJsonPropertyAnalyzer.IgnoreDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.Newtonsoft.JsonIgnoreDefinition());

        protected override string CodeFixTitle => CodeFixResources.NewtonsoftJsonIgnoreCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.NewtonsoftJsonIgnoreCodeFixTitle);
    }
}
