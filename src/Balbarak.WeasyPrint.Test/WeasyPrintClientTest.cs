using Balbarak.WeasyPrint.Test.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Balbarak.WeasyPrint.Test
{
    public class WeasyPrintClientTest
    {
        private readonly string _inputFolder = @"C:\Repos\WeasyPrint-netcore\src\Balbarak.WeasyPrint.Test\inputs";
        private readonly string _outputFolder = @"C:\Repos\WeasyPrint-netcore\src\Balbarak.WeasyPrint.Test\outputs";

        [Fact]
        public async Task Should_Create_Pdf_From_Input_Text_Async()
        {
            var trace = new DebugTraceWriter();

            using (WeasyPrintClient client = new WeasyPrintClient(trace))
            {
                var data = await client.GeneratePdfAsync("<h1>Hello World </h1>");

                Assert.NotNull(data);
            }
        }

        [Fact]
        public void Should_Create_Pdf_From_Input_Text()
        {
            var trace = new DebugTraceWriter();

            using (WeasyPrintClient client = new WeasyPrintClient(trace))
            {
                var data = client.GeneratePdf("<h1>Hello World </h1>");

                Assert.NotNull(data);
            }
        }

        [Fact]
        public void Should_Create_Pdf_From_Input_File()
        {
            var trace = new DebugTraceWriter();

            var input = $"{_inputFolder}\\complex.html";
            var output = $"{_outputFolder}\\output.pdf";

            using (WeasyPrintClient client = new WeasyPrintClient(trace))
            {
                client.GeneratePdf(input,output);

            }
        }
    }
}
