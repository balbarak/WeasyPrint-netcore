using Balbarak.WeasyPrint.Test.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace Balbarak.WeasyPrint.Test
{
    public class WeasyPrintClientTest
    {

        [Fact]
        public async Task Should_Create_Pdf_From_Input_Text()
        {
            var trace = new DebugTraceWriter();

            using (WeasyPrintClient client = new WeasyPrintClient(trace))
            {
                await client.GeneratePdfAsync("<h1>Hello World </h1>");
            }
        }
    }
}
