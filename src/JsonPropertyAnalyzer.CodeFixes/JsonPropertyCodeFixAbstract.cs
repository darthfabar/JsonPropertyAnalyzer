using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
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

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SystemTextJsonPropertyCodeFix)), Shared]
    public class SystemTextJsonPropertyCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SystemTextJsonPropertyAnalyzer.PropertyNameDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.SystemTextJson.JsonPropertyNameDefinition());

        protected override string CodeFixTitle => CodeFixResources.SystemTextJsonPropertyCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.SystemTextJsonPropertyCodeFixTitle);
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(SystemTextJsonIgnoreCodeFix)), Shared]
    public class SystemTextJsonIgnoreCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(SystemTextJsonPropertyAnalyzer.IgnoreDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.SystemTextJson.JsonIgnoreDefinition());

        protected override string CodeFixTitle => CodeFixResources.SystemTextJsonIgnoreCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.SystemTextJsonIgnoreCodeFixTitle);
    }


    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NewtonsoftJsonPropertyCodeFix)), Shared]
    public class NewtonsoftJsonPropertyCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NewtonsoftJsonPropertyAnalyzer.PropertyNameDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.Newtonsoft.JsonPropertyDefinition());

        protected override string CodeFixTitle => CodeFixResources.SystemTextJsonPropertyCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.SystemTextJsonPropertyCodeFixTitle);
    }

    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NewtonsoftJsonIgnoreCodeFix)), Shared]
    public class NewtonsoftJsonIgnoreCodeFix : JsonPropertyCodeFixAbstract
    {
        public override ImmutableArray<string> FixableDiagnosticIds => ImmutableArray.Create(NewtonsoftJsonPropertyAnalyzer.IgnoreDiagnosticId);

        protected override PropertyDeclarationSyntaxAttributeHelper _jsonAttributeHelper => new PropertyDeclarationSyntaxAttributeHelper(new Definitions.Newtonsoft.JsonIgnoreDefinition());

        protected override string CodeFixTitle => CodeFixResources.SystemTextJsonIgnoreCodeFixTitle;
        protected override string CodeFixTitleName => nameof(CodeFixResources.SystemTextJsonIgnoreCodeFixTitle);
    }
}
