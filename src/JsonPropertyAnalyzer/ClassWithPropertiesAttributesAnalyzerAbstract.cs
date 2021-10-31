using JsonPropertyAnalyzer.Definitions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Linq;

namespace JsonPropertyAnalyzer
{
    public abstract class ClassWithPropertiesAttributesAnalyzerAbstract : DiagnosticAnalyzer
    {
        protected const string Category = "Naming";

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
            context.EnableConcurrentExecution();
            context.RegisterSymbolAction(AnalyzeSymbol, SymbolKind.NamedType);
        }

        protected void AnalyzeSymbol(SymbolAnalysisContext context)
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
                var hasJsonPropertyAttribute = property.GetAttributes().Any(w => w.AttributeClass.Name == PropertyNameAttribute.AttributeName);
                var hasJsonIgnoreAttribute = property.GetAttributes().Any(w => w.AttributeClass.Name == IgnoreAttribute.AttributeName);

                if (hasJsonPropertyAttribute || hasJsonIgnoreAttribute) continue;
                hasMissingAttributes = true;

                if (!hasJsonPropertyAttribute)
                {
                    var diagnostic = Diagnostic.Create(GetPropertyRuleLevel(), property.Locations[0], property.Name);
                    context.ReportDiagnostic(diagnostic);
                }
                if (!hasJsonIgnoreAttribute)
                {
                    var diagnostic = Diagnostic.Create(GetPropertyIgnoreRuleLevel(), property.Locations[0], property.Name);
                    context.ReportDiagnostic(diagnostic);
                }
            }

            if (!hasMissingAttributes) return;

            var diagnosticClass = Diagnostic.Create(GetClassRuleLevel(), namedTypeSymbol.Locations[0], namedTypeSymbol.Name);
            context.ReportDiagnostic(diagnosticClass);
        }

        protected abstract IJsonAttribute IgnoreAttribute { get; }
        protected abstract IJsonAttribute PropertyNameAttribute { get; }

        protected abstract DiagnosticDescriptor GetClassRuleLevel();
        protected abstract DiagnosticDescriptor GetPropertyRuleLevel();
        protected abstract DiagnosticDescriptor GetPropertyIgnoreRuleLevel();
    }
}
