#nullable enable
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace JsonPropertyAnalyzer.CodeFixes
{
    public abstract class JsonAttributesInClassCodeFixAbstract : CodeFixProvider
    {
        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

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
}
