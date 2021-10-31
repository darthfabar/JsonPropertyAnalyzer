using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Linq;
using System.Threading.Tasks;

namespace JsonPropertyAnalyzer.CodeFixes
{
    public abstract class JsonPropertyCodeFixAbstract : CodeFixProvider
    {
        protected abstract PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper { get; }
        protected abstract string CodeFixTitle { get; }
        protected abstract string CodeFixTitleName { get; }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var dec = root.FindToken(diagnosticSpan.Start).Parent.FirstAncestorOrSelf<PropertyDeclarationSyntax>();
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<PropertyDeclarationSyntax>().FirstOrDefault();
            if (declaration is null) return;

            var propertyNodes = new[] { declaration };

            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixTitle,
                    createChangedSolution: c => _jsonAttributeHelper.AddCustomAttributes(context.Document, propertyNodes, c),
                    equivalenceKey: CodeFixTitleName),
                diagnostic);
        }
        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }
    }
}
