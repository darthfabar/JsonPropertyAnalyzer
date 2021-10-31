#nullable enable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using System.Collections.Immutable;
using System.Composition;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NewtonsoftJsonAttributesInClassCodeFix)), Shared]
    public class NewtonsoftJsonAttributesInClassCodeFix : JsonAttributesInClassCodeFixAbstract
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(NewtonsoftJsonPropertyAnalyzer.ClassWithPropertiesDiagnosticId); }
        }

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonIgnorePropertiesHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.Newtonsoft.JsonIgnoreDefinition());

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonPropertyNamePropertiesHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.Newtonsoft.JsonPropertyDefinition());


        protected override string CodeFixTitle => CodeFixResources.NewtonsoftJsonClassCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.NewtonsoftJsonClassCodeFixTitle);
    }
}
