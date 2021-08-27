using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Text;

namespace SampleAnalyzer.Diagnostics
{
    internal class Descriptors
    {
        internal static Diagnostic AC0001_MultiplyOverflowInt32(SyntaxNode node)
            => Diagnostic.Create(AC0001_MultiplyOverflowInt32_Descriptor, node.GetLocation(), node.ToString());
        internal static readonly DiagnosticDescriptor AC0001_MultiplyOverflowInt32_Descriptor = new(
            "AC0001",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0001_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0001_AC0002_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Overflow",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
            );
        internal static Diagnostic AC0002_LeftShiftOverflowInt32(SyntaxNode node)
            => Diagnostic.Create(AC0002_LeftShiftOverflowInt32_Descriptor, node.GetLocation(), node.ToString());
        internal static readonly DiagnosticDescriptor AC0002_LeftShiftOverflowInt32_Descriptor = new(
            "AC0002",
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0002_Title),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            new LocalizableResourceString(
                nameof(DiagnosticsResources.AC0001_AC0002_MessageFormat),
                DiagnosticsResources.ResourceManager,
                typeof(DiagnosticsResources)),
            "Overflow",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true
            );
    }
}
