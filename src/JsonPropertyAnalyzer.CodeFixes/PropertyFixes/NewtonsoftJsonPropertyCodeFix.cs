using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NewtonsoftJsonPropertyCodeFix)), Shared]
    public class NewtonsoftJsonPropertyCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NewtonsoftJsonPropertyAnalyzer.PropertyNameDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.Newtonsoft.JsonPropertyDefinition());

        protected override string CodeFixTitle => CodeFixResources.NewtonsoftJsonPropertyCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.NewtonsoftJsonPropertyCodeFixTitle);
    }
}
