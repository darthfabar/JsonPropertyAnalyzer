using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(JsonPropertyNameCodeFix)), Shared]
    public class JsonPropertyNameCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SystemTextJsonPropertyAnalyzer.PropertyNameDiagnosticId); }
        }

        public sealed override FixAllProvider GetFixAllProvider()
        {
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/FixAllProvider.md for more information on Fix All Providers
            return WellKnownFixAllProviders.BatchFixer;
        }

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var dec = root.FindToken(diagnosticSpan.Start).Parent.FirstAncestorOrSelf<PropertyDeclarationSyntax>();
            var declaration = root.FindToken(diagnosticSpan.Start).Parent.AncestorsAndSelf().OfType<PropertyDeclarationSyntax>().FirstOrDefault();
            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitle,
                    createChangedSolution: c => AddJsonPropertyAttribute(context.Document, declaration, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitle)),
                diagnostic);
        }
        private async Task<Solution> AddJsonPropertyAttribute(Document document, PropertyDeclarationSyntax typeDecl, CancellationToken cancellationToken)
        {
            // Compute new uppercase name.
            var identifierToken = typeDecl.Identifier;
            var propetyName = identifierToken.Text;
            var jsonPropertyNameText = char.ToLower(propetyName[0]) + propetyName.Substring(1);

            return await AddCustomAttribute(document, typeDecl, jsonPropertyNameText, cancellationToken);
        }
        private async Task<Solution> AddCustomAttribute(Document document, PropertyDeclarationSyntax typeDecl, string jsonPropertyNameText, CancellationToken cancellationToken)
        {
            var name = SyntaxFactory.ParseName("JsonPropertyName");
            var arguments = SyntaxFactory.ParseAttributeArgumentList($"(\"{jsonPropertyNameText}\")");
            var attribute = SyntaxFactory.Attribute(name, arguments);

            var attributeList = new SeparatedSyntaxList<AttributeSyntax>();
            attributeList = attributeList.Add(attribute);
            var list = SyntaxFactory.AttributeList(attributeList);

            var root = await document.GetSyntaxRootAsync(cancellationToken);

            var attributes = typeDecl.AttributeLists.Add(list);

            return document.WithSyntaxRoot(
                root.ReplaceNode(
                    typeDecl,
                    typeDecl.WithAttributeLists(attributes)
                )).Project.Solution;
        }

    }
}
