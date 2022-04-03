using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = JsonPropertyAnalyzer.Test.CSharpCodeFixVerifier<
    JsonPropertyAnalyzer.SystemTextJsonPropertyAnalyzer,
    JsonPropertyAnalyzer.CodeFixes.SystemTextJsonAttributesInClassCodeFix>;

namespace JsonPropertyAnalyzer.Test
{
    [TestClass]
    public class Analyzer1UnitTest
    {
        //No diagnostics expected to show up
        [TestMethod]
        public async Task TestMethod1()
        {
            var test = @"";

            await VerifyCS.VerifyAnalyzerAsync(test);
        }

        // TODO add proper unit tests
        /*
                //Diagnostic and CodeFix both triggered and checked for
                [TestMethod]
                public async Task TestMethod2()
                {
                    var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                class {|#0:TypeName|}
                {   
                    public int Property1 {get;set;}
                    public int Property2 {get;set;}
                }
            }";

                    var fixtest = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                class TYPENAME
                {   
                    public int Property1 {get;set;}
                    public int Property2 {get;set;}
                }
            }";
                    var expected = VerifyCS.Diagnostic(SystemTextJsonPropertyAnalyzer.PropertyNameDiagnosticId).WithLocation(0).WithArguments("TypeName");
                    await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
                }


                //Diagnostic and CodeFix both triggered and checked for
                [TestMethod]
                public async Task TestMethod3()
                {
                    var test = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                class {|#0:TypeName|}
                {   
                    public int Property1 {get;set;}
                    public int Property2 {get;set;}
                }
            }";

                    var fixtest = @"
            using System;
            using System.Collections.Generic;
            using System.Linq;
            using System.Text;
            using System.Threading.Tasks;
            using System.Diagnostics;

            namespace ConsoleApplication1
            {
                class TYPENAME
                {   
                    public int Property1 {get;set;}
                    public int Property2 {get;set;}
                }
            }";
                    var ab = new DiagnosticDescriptor(NewtonsoftJsonPropertyAnalyzer.PropertyNameDiagnosticId, string.Empty, string.Empty, string.Empty, DiagnosticSeverity.Info, true);
                    //var a = VerifyCS.Diagnostic();
                    var expected = VerifyCS.Diagnostic(ab).WithLocation(0).WithArguments("TypeName");
                    await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
                }
        
        [TestMethod]
        public async Task TestMethod3()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class {|#0:TypeName|}
        {   
            public int Property1 {get;set;}
            public int Property2 {get;set;}
        }
    }";

            var ab = new DiagnosticDescriptor(NewtonsoftJsonPropertyAnalyzer.PropertyNameDiagnosticId, string.Empty, string.Empty, string.Empty, DiagnosticSeverity.Info, true);

            await VerifyCS.VerifyAnalyzerAsync(test, 
                new Microsoft.CodeAnalysis.Testing.DiagnosticResult(SystemTextJsonPropertyAnalyzer.PropertyNameDiagnosticId, DiagnosticSeverity.Info),
                new Microsoft.CodeAnalysis.Testing.DiagnosticResult(SystemTextJsonPropertyAnalyzer.IgnoreDiagnosticId, DiagnosticSeverity.Info),
                new Microsoft.CodeAnalysis.Testing.DiagnosticResult(SystemTextJsonPropertyAnalyzer.PropertyNameDiagnosticId, DiagnosticSeverity.Info),
                new Microsoft.CodeAnalysis.Testing.DiagnosticResult(SystemTextJsonPropertyAnalyzer.IgnoreDiagnosticId, DiagnosticSeverity.Info),
                new Microsoft.CodeAnalysis.Testing.DiagnosticResult(SystemTextJsonPropertyAnalyzer.ClassWithPropertiesDiagnosticId, DiagnosticSeverity.Info))
                ;
            //var a = VerifyCS.Diagnostic();
            //var expected = VerifyCS.Diagnostic(ab).WithLocation(0).WithArguments("TypeName");
            //await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }*/

    }

}
