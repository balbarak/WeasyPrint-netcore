using System;
using System.IO;
using System.Threading.Tasks;
using Balbarak.WeasyPrint;
namespace WeasyPrintWrapper
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var trace = new ConsoleTraceWriter();

            using (WeasyPrintClient client = new WeasyPrintClient(trace))
            {

                var html = "<!DOCTYPE html><html><body><h1>Hello World</h1></body></html>";

                var data = await client.GeneratePdfAsync(html);

                //File.WriteAllBytes("test.pdf", data);
            }

            Console.ReadLine();
        }

    }
}
