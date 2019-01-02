using System;
using System.IO;
using Xunit;

namespace Balbarak.WeasyPrint.Test
{
    public class WeasyPrintClientTest
    {
        [Fact]
        public void SetupGtkPathTest()
        {
            var html = File.ReadAllText("index.html");

            using (WeasyPrintClient client = new WeasyPrintClient())
            {
                //client.TestWeasy();

                var result = client.GeneratePdf(html);

                var path = Directory.GetCurrentDirectory();


                File.WriteAllBytes($"{Directory.GetCurrentDirectory()}\\output.pdf", result);
            }
        }
    }
}
