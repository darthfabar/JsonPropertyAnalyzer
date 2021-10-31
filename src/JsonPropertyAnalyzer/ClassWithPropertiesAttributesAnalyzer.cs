using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;
using System.Linq;

namespace JsonPropertyAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class ClassWithPropertiesAttributesAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = "PropertiesWithMissingAttributes";
        public const string ClassWithPropertiesDiagnosticId = "ClassWithMissingPropertiesAttributes";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Localizing%20Analyzers.md for more on localization
        private static readonly LocalizableString TitleProperty = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatProperty = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionProperty = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString TitleClass = new LocalizableResourceString(nameof(Resources.AnalyzerClassTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatClass = new LocalizableResourceString(nameof(Resources.AnalyzerClassMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionClass = new LocalizableResourceString(nameof(Resources.AnalyzerClassDescription), Resources.ResourceManager, typeof(Resources));
        private const string Category = "Naming";

        private static readonly DiagnosticDescriptor RulePropertyLevel = new DiagnosticDescriptor(DiagnosticId, TitleProperty, MessageFormatProperty, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionProperty);
        private static readonly DiagnosticDescriptor RuleClassLevel = new DiagnosticDescriptor(ClassWithPropertiesDiagnosticId, TitleClass, MessageFormatClass, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionClass);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(RulePropertyLevel, RuleClassLevel); } }

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();

            // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
            // See https://github.com/dotnet/roslyn/blob/main/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        private static void AnalyzeSymbol(SymbolAnalysisContext context)
        {
            var namedTypeSymbol = (INamedTypeSymbol)context.Symbol;
            var attributes = context.Symbol.GetAttributes();
            
            var publicProperties = namedTypeSymbol
                .GetMembers()
                .Where(w => w.DeclaredAccessibility == Accessibility.Public && w.Kind == SymbolKind.Property)
                .ToArray();

            var hasMissingAttributes = false;
            foreach (var property in publicProperties)
            {
                var hasJsonPropertyAttribute = property.GetAttributes().Any(w => w.AttributeClass.Name == "JsonPropertyNameAttribute");
                if (hasJsonPropertyAttribute) continue;
                hasMissingAttributes = true;
                var diagnostic = Diagnostic.Create(RulePropertyLevel, property.Locations[0], property.Name);

                context.ReportDiagnostic(diagnostic);
            }

            if (!hasMissingAttributes) return;

            var diagnosticClass = Diagnostic.Create(RuleClassLevel, namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
            context.ReportDiagnostic(diagnosticClass);
        }
    }
}
