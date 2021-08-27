using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using SampleAnalyzer.Diagnostics;

namespace SampleAnalyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class IntToLongAnalyzer : DiagnosticAnalyzer
    {
        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics
            => ImmutableArray.Create(
                Descriptors.AC0001_MultiplyOverflowInt32_Descriptor,
                Descriptors.AC0002_LeftShiftOverflowInt32_Descriptor);

        public override void Initialize(AnalysisContext context)
        {
            context.ConfigureGeneratedCodeAnalysis(
                GeneratedCodeAnalysisFlags.Analyze |
                GeneratedCodeAnalysisFlags.ReportDiagnostics);
            context.EnableConcurrentExecution();

            context.RegisterSyntaxNodeAction(AnalyzeIntToLongSyntaxNode,
                SyntaxKind.LeftShiftExpression, SyntaxKind.MultiplyExpression);
        }

        private void AnalyzeIntToLongSyntaxNode(SyntaxNodeAnalysisContext context)
        {
            var semanticModel = context.SemanticModel;
            var node = context.Node;

            var typeInfo = semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken);
            if (typeInfo.Type?.SpecialType != SpecialType.System_Int32)
                return;

            Diagnostic diagnostic = node.Kind() switch
            {
                SyntaxKind.MultiplyExpression => Descriptors.AC0001_MultiplyOverflowInt32(context.Node),
                SyntaxKind.LeftShiftExpression => Descriptors.AC0002_LeftShiftOverflowInt32(context.Node),
                _ => throw new InvalidOperationException(),
            };

            for (; node is not null; node = GetParent(node))
            {
                if (semanticModel.GetTypeInfo(node, cancellationToken: context.CancellationToken)
                    .ConvertedType?.SpecialType == SpecialType.System_Int64)
                {
                    context.ReportDiagnostic(diagnostic);
                    return;
                }
            }

            static SyntaxNode? GetParent(SyntaxNode node)
            {
                var parent = node.Parent;
                if (parent is BinaryExpressionSyntax or ParenthesizedExpressionSyntax)
                    return parent;
                return null;
            }
        }
    }
}
