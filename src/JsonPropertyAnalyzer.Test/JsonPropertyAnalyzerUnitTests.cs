using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using VerifyCS = JsonPropertyAnalyzer.Test.CSharpCodeFixVerifier<
    JsonPropertyAnalyzer.ClassWithPropertiesAttributesAnalyzer,
    JsonPropertyAnalyzer.CodeFixes.JsonPropertyNameCodeFix>;

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

            var expected = VerifyCS.Diagnostic("JsonPropertyAnalyzer").WithLocation(0).WithArguments("TypeName");
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

            var expected = VerifyCS.Diagnostic("JsonPropertyAnalyzer").WithLocation(0).WithArguments("TypeName");
            await VerifyCS.VerifyCodeFixAsync(test, expected, fixtest);
        }
    }

}
