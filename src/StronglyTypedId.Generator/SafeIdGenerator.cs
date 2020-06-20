using CodeGeneration.Roslyn;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StronglyTypedId.Generator
{
    public class SafeIdGenerator : IRichCodeGenerator
    {
        private readonly bool _generateJsonConverter;
        private readonly StronglyTypedIdJsonConverter _jsonProvider;
        private string _generatorFunction;
        private string _type;
        private string _version;
        private string _separator;

        public SafeIdGenerator(AttributeData attributeData)
        {
            if (attributeData == null) throw new ArgumentNullException(nameof(attributeData));
            var named = attributeData.NamedArguments.ToDictionary(o => o.Key, o => o.Value);
            if (named.TryGetValue("GeneratorFunction", out var typedConstant))
            {
                _generatorFunction = (string)typedConstant.Value;
            }
            else
            {
                _generatorFunction = "NewId";
            }

            if (named.TryGetValue("Separator", out typedConstant))
            {
                _separator = (string)typedConstant.Value;
            }
            else
            {
                _separator = "-";
            }

            _type = (string) attributeData.ConstructorArguments[0].Value;
            _version = (string)attributeData.ConstructorArguments[1].Value;
            _generateJsonConverter = (bool)attributeData.ConstructorArguments[2].Value;
            _jsonProvider = (StronglyTypedIdJsonConverter)attributeData.ConstructorArguments[3].Value;
        }

        public Task<SyntaxList<MemberDeclarationSyntax>> GenerateAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<RichGenerationResult> GenerateRichAsync(TransformationContext context, IProgress<Diagnostic> progress, CancellationToken cancellationToken)
        {
            var applyToClass = (StructDeclarationSyntax)context.ProcessingNode;
            SyntaxList<MemberDeclarationSyntax> stronglyTypedId = GetSyntax(applyToClass);

            // Figure out ancestry for the generated type, including nesting types and namespaces.
            var wrappedMembers = stronglyTypedId.WrapWithAncestors(context.ProcessingNode);

            return Task.FromResult(new RichGenerationResult
            {
                Members = wrappedMembers,
            });
        }

        private SyntaxList<MemberDeclarationSyntax> GetSyntax(StructDeclarationSyntax applyToClass)
        {
            return new SafeIdSyntaxTreeGenerator(_type, _version, _separator, _generatorFunction).CreateStronglyTypedIdSyntax(applyToClass, _generateJsonConverter, _jsonProvider);
        }

    }
}