#nullable enable
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;

namespace JsonPropertyAnalyzer.CodeFixes
{
    public abstract class JsonAttributesInClassCodeFixAbstract : CodeFixProvider
    {
        protected abstract PropertyDeclarationSyntaxAttributeHelper _jsonIgnorePropertiesHelper { get; }
        protected abstract PropertyDeclarationSyntaxAttributeHelper _jsonPropertyNamePropertiesHelper { get; }
        protected abstract string CodeFixTitle { get; }
        protected abstract string CodeFixTitleName { get; }
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root is null) return;

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var classToken = root.FindToken(diagnosticSpan.Start).Parent;

            if (classToken is null) return;

            var children = classToken.ChildNodes().ToArray();
            var propertyNodes = children.OfType<PropertyDeclarationSyntax>()
                .Where(_jsonIgnorePropertiesHelper.IsPublicProperty)
                .Where(w=> _jsonIgnorePropertiesHelper.MissesAttributes(w) && _jsonPropertyNamePropertiesHelper.MissesAttributes(w))
                .ToArray();


            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixTitle,
                    createChangedSolution: c => _jsonPropertyNamePropertiesHelper.AddCustomAttributes(context.Document, propertyNodes, c),
                    equivalenceKey: CodeFixTitleName),
                diagnostic);
        }
    }

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
