using System;
using System.Threading.Tasks;

namespace Balbarak.WeasyPrint
{
    public interface IWeasyPrintClient : IDisposable
    {
        event WeasyPrintClient.WeasyPrintEventHandler OnDataOutput;
        event WeasyPrintClient.WeasyPrintEventHandler OnDataError;
        byte[] GeneratePdf(string htmlText);
        Task<byte[]> GeneratePdfAsync(string htmlText);
        void GeneratePdf(string inputPathFile, string outputPathFile);
        Task GeneratePdfAsync(string inputPathFile, string outputPathFile);
        byte[] GeneratePdfFromUrl(string url);
        Task<byte[]> GeneratePdfFromUrlAsync(string url);
    }
}