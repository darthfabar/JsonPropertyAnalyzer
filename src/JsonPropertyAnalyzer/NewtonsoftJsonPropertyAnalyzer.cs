using JsonPropertyAnalyzer.Definitions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

using System.Collections.Immutable;

namespace JsonPropertyAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class NewtonsoftJsonPropertyAnalyzer : ClassWithPropertiesAttributesAnalyzerAbstract
    {
        public const string PropertyNameDiagnosticId = "PropertiesWithMissingJsonProperty";
        public const string IgnoreDiagnosticId = "PropertiesWithMissingNewtonsoftJsonIgnore";
        public const string ClassWithPropertiesDiagnosticId = "ClassWithMissingNewtonsoftJsonPropertiesAttributes";


        private static readonly LocalizableString TitlePropertyName = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonPropertyNameTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatPropertyName = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonPropertyNameMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionPropertyName = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonPropertyNameDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString TitlePropertyIgnore = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonIgnoreTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatPropertyIgnore = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonIgnoreMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionPropertyIgnore = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonIgnoreDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly LocalizableString TitleClass = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonClassTitle), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString MessageFormatClass = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonClassMessageFormat), Resources.ResourceManager, typeof(Resources));
        private static readonly LocalizableString DescriptionClass = new LocalizableResourceString(nameof(Resources.NewtonsoftJsonClassDescription), Resources.ResourceManager, typeof(Resources));

        private static readonly DiagnosticDescriptor RulePropertyLevel = new DiagnosticDescriptor(PropertyNameDiagnosticId, TitlePropertyName, MessageFormatPropertyName, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionPropertyName);
        private static readonly DiagnosticDescriptor RulePropertyIgnoreLevel = new DiagnosticDescriptor(IgnoreDiagnosticId, TitlePropertyIgnore, MessageFormatPropertyIgnore, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionPropertyIgnore);
        private static readonly DiagnosticDescriptor RuleClassLevel = new DiagnosticDescriptor(ClassWithPropertiesDiagnosticId, TitleClass, MessageFormatClass, Category, DiagnosticSeverity.Info, isEnabledByDefault: true, description: DescriptionClass);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(RuleClassLevel, RulePropertyIgnoreLevel, RulePropertyLevel);

        protected override IJsonAttribute IgnoreAttribute => new Definitions.Newtonsoft.JsonIgnoreDefinition();
        protected override IJsonAttribute PropertyNameAttribute => new Definitions.Newtonsoft.JsonPropertyDefinition();

        protected override DiagnosticDescriptor GetClassRuleLevel() => RuleClassLevel;

        protected override DiagnosticDescriptor GetPropertyIgnoreRuleLevel() => RulePropertyIgnoreLevel;

        protected override DiagnosticDescriptor GetPropertyRuleLevel() => RulePropertyLevel;
    }
}
