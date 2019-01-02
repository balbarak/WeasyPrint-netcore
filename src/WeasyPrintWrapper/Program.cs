using System;
using System.IO;
using Balbarak.WeasyPrint;
namespace WeasyPrintWrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WeasyPrintClient client = new WeasyPrintClient())
            {
                client.OnDataOutput += OnDataOutput;
                client.OnDataError += OnDataError;

                var html = "<!DOCTYPE html><html><body><h1>Hello World</h1></body></html>";

                var data = client.GeneratePdf(html);

                File.WriteAllBytes("test.pdf", data);
            }

            Console.ReadLine();
        }

        private static void OnDataOutput(OutputEventArgs e)
        {
            Console.WriteLine(e.Data);
        }

        private static void OnDataError(OutputEventArgs e)
        {
            Console.WriteLine(e.Data);
        }
    }
}
