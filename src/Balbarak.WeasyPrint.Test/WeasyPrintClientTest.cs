using System;
using Xunit;

namespace Balbarak.WeasyPrint.Test
{
    public class WeasyPrintClientTest
    {
        [Fact]
        public void SetupGtkPathTest()
        {
            using (WeasyPrintClient client = new WeasyPrintClient())
            {
                client.TestWeasy();
            }
        }
    }
}
