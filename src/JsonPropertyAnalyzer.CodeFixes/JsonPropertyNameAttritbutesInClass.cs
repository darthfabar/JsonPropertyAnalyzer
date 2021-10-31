using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JsonPropertyAnalyzer.CodeFixes
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(JsonPropertyNameCodeFix)), Shared]
    public class JsonPropertyNameAttritbutesInClassCodeFix : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds
        {
            get { return ImmutableArray.Create(SystemTextJsonPropertyAnalyzer.ClassWithPropertiesDiagnosticId); }
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
            var classToken = root.FindToken(diagnosticSpan.Start).Parent;
            var children = classToken.ChildNodes().ToArray();
            var propertyNodes = children.OfType<PropertyDeclarationSyntax>()
                .Where(IsPublicProperty)
                .Where(HasAlreadyAttributes)
                .ToArray();


            context.RegisterCodeFix(
                CodeAction.Create(
                    title: CodeFixResources.CodeFixTitleClass,
                    createChangedSolution: c => AddJsonPropertyAttributes(context.Document, propertyNodes, c),
                    equivalenceKey: nameof(CodeFixResources.CodeFixTitleClass)),
                diagnostic);
        }

        private static bool HasAlreadyAttributes(PropertyDeclarationSyntax w)
        {
            return !w.AttributeLists.Any() || !w.AttributeLists.Any(a =>
            {
                return a.Attributes.Any(att =>
                {
                    var text = att.Name.GetText();
                    return text.ToString().EndsWith("JsonPropertyName");
                });
            });
        }

        private static bool IsPublicProperty(PropertyDeclarationSyntax w)
        {
            return w.Modifiers.Where(a => string.Equals(a.ValueText, "public", StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        private async Task<Solution> AddJsonPropertyAttributes(Document document, PropertyDeclarationSyntax[] typeDecl, CancellationToken cancellationToken)
        {
            return await AddCustomAttributes(document, typeDecl, cancellationToken);
        }

        private static string CreateJsonName(PropertyDeclarationSyntax typeDecl)
        {
            var identifierToken = typeDecl.Identifier;
            var propetyName = identifierToken.Text;
            var jsonPropertyNameText = char.ToLower(propetyName[0]) + propetyName.Substring(1);
            return jsonPropertyNameText;
        }

        private async Task<Solution> AddCustomAttributes(Document document, PropertyDeclarationSyntax[] typeDecl, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(
                root.ReplaceNodes(
                    typeDecl,
                    (current, after) => AddJsonAttribute(current, after)
                )).Project.Solution;
        }

        private PropertyDeclarationSyntax AddJsonAttribute(PropertyDeclarationSyntax current, PropertyDeclarationSyntax after)
        {
            var jsonPropertyNameText = CreateJsonName(current);
            var name = SyntaxFactory.ParseName("JsonPropertyName");
            var arguments = SyntaxFactory.ParseAttributeArgumentList($"(\"{jsonPropertyNameText}\")");
            var attribute = SyntaxFactory.Attribute(name, arguments);

            var attributeList = new SeparatedSyntaxList<AttributeSyntax>();
            attributeList = attributeList.Add(attribute);
            var list = SyntaxFactory.AttributeList(attributeList);
            var attributes = current.AttributeLists.Add(list);

            after = current.WithAttributeLists(attributes);
            return after;
        }

    }
}
