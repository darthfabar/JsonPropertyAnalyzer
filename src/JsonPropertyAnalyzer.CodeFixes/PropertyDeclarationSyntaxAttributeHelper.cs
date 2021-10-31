#nullable enable
using JsonPropertyAnalyzer.Definitions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace JsonPropertyAnalyzer.CodeFixes
{
    public class PropertyDeclarationSyntaxAttributeHelper
    {
        private readonly IJsonAttribute _jsonAttribute;

        public PropertyDeclarationSyntaxAttributeHelper(IJsonAttribute jsonAttribute)
        {
            _jsonAttribute = jsonAttribute;
        }

        public bool MissesAttributes(PropertyDeclarationSyntax w)
        {
            return !w.AttributeLists.Any() || !w.AttributeLists.Any(a =>
            {
                return a.Attributes.Any(att =>
                {
                    var text = att.Name.GetText();
                    return text.ToString().EndsWith(_jsonAttribute.AttributeDisplayName);
                });
            });
        }

        public bool IsPublicProperty(PropertyDeclarationSyntax w)
        {
            return w.Modifiers.Where(a => string.Equals(a.ValueText, "public", StringComparison.InvariantCultureIgnoreCase)).Any();
        }

        public async Task<Solution> AddCustomAttributes(Document document, PropertyDeclarationSyntax[] typeDecl, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            return document.WithSyntaxRoot(
                root!.ReplaceNodes(
                    typeDecl,
                    (current, after) => AddJsonAttribute(current, after)
                )!).Project.Solution;
        }

        public PropertyDeclarationSyntax AddJsonAttribute(PropertyDeclarationSyntax current, PropertyDeclarationSyntax after)
        {
            var name = SyntaxFactory.ParseName(_jsonAttribute.AttributeDisplayName);
            
            var arguments = GetArguments(current);
            var attribute = SyntaxFactory.Attribute(name, arguments);

            var attributeList = new SeparatedSyntaxList<AttributeSyntax>();
            attributeList = attributeList.Add(attribute);
            var list = SyntaxFactory.AttributeList(attributeList);
            var attributes = current.AttributeLists.Add(list);

            after = current.WithAttributeLists(attributes);
            return after;
        }

        private AttributeArgumentListSyntax? GetArguments(PropertyDeclarationSyntax current)
        {
            if (!_jsonAttribute.HasParameter) return null;
            var jsonPropertyNameText = CreateJsonNamePascalCase(current);
            var arguments = SyntaxFactory.ParseAttributeArgumentList($"(\"{jsonPropertyNameText}\")");
            return arguments;
        }

        public static string CreateJsonNamePascalCase(PropertyDeclarationSyntax typeDecl)
        {
            var identifierToken = typeDecl.Identifier;
            var propetyName = identifierToken.Text;
            var jsonPropertyNameText = char.ToLower(propetyName[0]) + propetyName.Substring(1);
            return jsonPropertyNameText;
        }
    }
}
