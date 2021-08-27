using System;
using System.Threading.Tasks;
using Xunit;
using VerifyCS = SampleAnalyzer.Test.CSharpCodeFixVerifier<
    SampleAnalyzer.IntToLongAnalyzer,
    SampleAnalyzer.IntToLongCodeFixProvider>;

namespace SampleAnalyzer.Test
{
    public class IntToLongAnalyzerTest
    {
        [Fact]
        public async Task Analyze()
        {
            var source = @"using System;

class Program
{
    static void WriteLong(long num) => Console.WriteLine(num);
    void Write()
    {
        var i = 1;
        WriteLong(1 * 2);
        WriteLong(1 << 2);
        WriteLong(i * 2);
        WriteLong(i << 2);
    }
}";
            var fixedSource = @"using System;

class Program
{
    static void WriteLong(long num) => Console.WriteLine(num);
    void Write()
    {
        var i = 1;
        WriteLong(1L * 2);
        WriteLong(1L << 2);
        WriteLong((long)i * 2);
        WriteLong((long)i << 2);
    }
}";
            await VerifyCS.VerifyCodeFixAsync(
                source,
                new[]
                {
                    VerifyCS.Diagnostic("AC0001").WithSpan(9, 19, 9, 24).WithArguments("1 * 2"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(10, 19, 10, 25).WithArguments("1 << 2"),
                    VerifyCS.Diagnostic("AC0001").WithSpan(11, 19, 11, 24).WithArguments("i * 2"),
                    VerifyCS.Diagnostic("AC0002").WithSpan(12, 19, 12, 25).WithArguments("i << 2"),
                },
                fixedSource
            );
        }
    }
}
