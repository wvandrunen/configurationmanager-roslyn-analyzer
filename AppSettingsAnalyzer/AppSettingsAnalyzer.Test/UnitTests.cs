using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using TestHelper;
using AppSettingsAnalyzer;

namespace AppSettingsAnalyzer.Test
{
  [TestClass]
  public class UnitTest : CodeFixVerifier
  {

    

    //No diagnostics expected to show up
    [TestMethod]
    public void TestMethod1()
    {
      var test = @"";

      VerifyCSharpDiagnostic(test);
    }    

    //Diagnostic and CodeFix both triggered and checked for
    [TestMethod]
    public void TestMethod2()
    {

      var test = @"
      using System;
      using System.Configuration;
    
      namespace ConsoleApplication1
      {         
          ConfigurationManager.AppSettings.Add(""test"", ""test"");  

          public class TypeName
          {   
            var setting = ConfigurationManager.AppSettings[""test""];
          }
      }";

      VerifyCSharpDiagnostic(test, new DiagnosticResult
      {
        Id = "AppSettingsAnalyzer",
        Locations = {},
        Message = "",
        Severity = DiagnosticSeverity.Error
      });

      //  ConfigurationManager.AppSettings["sadasd"];
    }

    protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
    {
      return new AppSettingsAnalyzerAnalyzer();
    }
  }
}