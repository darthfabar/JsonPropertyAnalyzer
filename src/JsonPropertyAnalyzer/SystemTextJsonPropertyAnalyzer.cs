using JsonPropertyAnalyzer.Definitions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using System.Collections.Immutable;

namespace JsonPropertyAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class SystemTextJsonPropertyAnalyzer : ClassWithPropertiesAttributesAnalyzerAbstract
    {
        public const string PropertyNameDiagnosticId = "JS01";
        public const string IgnoreDiagnosticId = "JS02";
        public const string ClassWithPropertiesDiagnosticId = "JS03";


        private static readonly LocalizableString TitlePropertyName = new LocalizableResourceString(nameof(Resources.SystemTextJsonPropertyNameTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatPropertyName = new LocalizableResourceString(nameof(Resources.SystemTextJsonPropertyNameMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionPropertyName = new LocalizableResourceString(nameof(Resources.SystemTextJsonPropertyNameDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString TitlePropertyIgnore = new LocalizableResourceString(nameof(Resources.SystemTextJsonIgnoreTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatPropertyIgnore = new LocalizableResourceString(nameof(Resources.SystemTextJsonIgnoreMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionPropertyIgnore = new LocalizableResourceString(nameof(Resources.SystemTextJsonIgnoreDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString TitleClass = new LocalizableResourceString(nameof(Resources.SystemTextJsonClassTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatClass = new LocalizableResourceString(nameof(Resources.SystemTextJsonClassMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionClass = new LocalizableResourceString(nameof(Resources.SystemTextJsonClassDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor RulePropertyLevel = new DiagnosticDescriptor(PropertyNameDiagnosticId, TitlePropertyName, MessageFormatPropertyName, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionPropertyName);
        private static readonly DiagnosticDescriptor RulePropertyIgnoreLevel = new DiagnosticDescriptor(IgnoreDiagnosticId, TitlePropertyIgnore, MessageFormatPropertyIgnore, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionPropertyIgnore);
        private static readonly DiagnosticDescriptor RuleClassLevel = new DiagnosticDescriptor(ClassWithPropertiesDiagnosticId, TitleClass, MessageFormatClass, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionClass);
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(RuleClassLevel, RulePropertyIgnoreLevel, RulePropertyLevel);
        protected override IJsonAttribute IgnoreAttribute => new Definitions.SystemTextJson.JsonIgnoreDefinition();
        protected override IJsonAttribute PropertyNameAttribute => new Definitions.SystemTextJson.JsonPropertyNameDefinition();

        protected override DiagnosticDescriptor GetClassRuleLevel() => RuleClassLevel;

        protected override DiagnosticDescriptor GetPropertyIgnoreRuleLevel() => RulePropertyIgnoreLevel;

        protected override DiagnosticDescriptor GetPropertyRuleLevel() => RulePropertyLevel;
    }
}
