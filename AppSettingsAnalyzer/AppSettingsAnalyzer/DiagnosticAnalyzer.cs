using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.FindSymbols;

namespace AppSettingsAnalyzer
{
  [DiagnosticAnalyzer(LanguageNames.CSharp)]
  public class AppSettingsAnalyzerAnalyzer : DiagnosticAnalyzer
  {
    public const string DiagnosticId = "AppSettingsAnalyzer";

    // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
    // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
    private const string Category = "Naming";

    private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

    public override void Initialize(AnalysisContext context)
    {
      // TODO: Consider registering other actions that act on syntax instead of or in addition to symbols
      // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Analyzer%20Actions%20Semantics.md for more information

      context.RegisterSyntaxNodeAction(AnalyzeVariableAssigment, SyntaxKind.ElementAccessExpression);
    }

    private void AnalyzeVariableAssigment(SyntaxNodeAnalysisContext context)
    {
      //var syntaxTree = context.Node.SyntaxTree;
      var parts = context.Node.DescendantNodes().OfType<IdentifierNameSyntax>();
      

      var configurationManagerMaybe = parts.FirstOrDefault();
      var isConfigurationManager = (configurationManagerMaybe.Identifier.ValueText == "ConfigurationManager");

      var appSettingsMaybe = parts.Skip(1).Take(1).FirstOrDefault();
      var isAppSettings = appSettingsMaybe.Identifier.ValueText.Equals("AppSettings");
                           

      if (isAppSettings && isConfigurationManager)
      {
        var diagnostic = Diagnostic.Create(Rule, configurationManagerMaybe.GetLocation());

        context.ReportDiagnostic(diagnostic);
      }
    }
 
  }
}
