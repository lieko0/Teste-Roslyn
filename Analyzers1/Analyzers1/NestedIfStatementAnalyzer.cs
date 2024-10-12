using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Analyzers1;

/// <summary>
/// A simple code analyzer that checks the number of nested if statements inside
/// methods.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]

public class NestedIfStatementAnalyzer : DiagnosticAnalyzer
{
    private const string DiagnosticId = "AB0001";
    private const int MaxAllowedNestedIfStmts = 3;
    
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AB0001Title),
        Resources.ResourceManager, typeof(Resources));
    
    private static readonly LocalizableString Description =
        new LocalizableResourceString(nameof(Resources.AB0001Description), Resources.ResourceManager,
            typeof(Resources));
    
    private static readonly LocalizableString MessageFormat =
        new LocalizableResourceString(nameof(Resources.AB0001MessageFormat), Resources.ResourceManager,
            typeof(Resources));
    
    private const string Category = "Design";
    
    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId, Title, MessageFormat, Category,
        DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
    
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; } =
        ImmutableArray.Create(Rule);
    
    public override void Initialize(AnalysisContext context)
    {
        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();
        context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.MethodDeclaration);
    }

    private void Analyze(SyntaxNodeAnalysisContext context)
    {
        if(!context.Node.IsKind(SyntaxKind.MethodDeclaration))
        {
            return;
        }
        
        var method = (MethodDeclarationSyntax) context.Node;
        
        bool isAboveLimit = this.IsNestedIfStmntAboveLimit(method);

        if (isAboveLimit)
        {
            var diagnostic = Diagnostic.Create(Rule, method.GetLocation(), MaxAllowedNestedIfStmts);
            context.ReportDiagnostic(diagnostic);
        }
    }
    
    private bool IsNestedIfStmntAboveLimit(MethodDeclarationSyntax methodDeclaration)
    {
        var ifStatements = methodDeclaration.DescendantNodes().OfType<IfStatementSyntax>();

        foreach (var ifStatement in ifStatements)
        {
            if (ifStatement.Ancestors().OfType<IfStatementSyntax>().Count() + 1 > MaxAllowedNestedIfStmts);
            return true;
        }

        return false;
    }
}