using Balbarak.WeasyPrint.Internals;
using Balbarak.WeasyPrint.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

namespace Balbarak.WeasyPrint
{
    public class WeasyPrintClient : IWeasyPrintClient
    {
        private readonly FilesManager _fileManager;
        private readonly ProcessInvoker _invoker;
        private readonly ITraceWriter _trace;
        private Dictionary<string, string> _environmentVariables;

        public delegate void WeasyPrintEventHandler(OutputEventArgs e);
        public event WeasyPrintEventHandler OnDataOutput;
        public event WeasyPrintEventHandler OnDataError;

        public WeasyPrintClient()
        {
            _fileManager = new FilesManager();

            SetEnviromentVariables();

            _invoker = new ProcessInvoker(_environmentVariables);
        }

        public WeasyPrintClient(ITraceWriter traceWriter) : this()
        {
            _invoker = new ProcessInvoker(_environmentVariables, traceWriter);

            _trace = traceWriter;
        }

        public byte[] GeneratePdf(string htmlText)
        {
            byte[] result;

            try
            {
                result = GeneratePdfInternal(htmlText).GetAwaiter().GetResult();

                return result;
            }
            catch (Exception ex)
            {
                OnDataError?.Invoke(new OutputEventArgs(ex.ToString()));

                throw new WeasyPrintException(ex.Message, ex);
            }

        }

        public async Task<byte[]> GeneratePdfAsync(string htmlText)
        {
            byte[] result;

            try
            {
                result = await GeneratePdfInternal(htmlText).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());

                throw new WeasyPrintException(ex.Message, ex);
            }
        }

        public void GeneratePdf(string inputPathFile, string outputPathFile)
        {
            try
            {

                GeneratePdfInternal(inputPathFile, outputPathFile).GetAwaiter().GetResult();

            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
                throw new WeasyPrintException(ex.Message, ex);
            }

        }

        public async Task GeneratePdfAsync(string inputPathFile, string outputPathFile)
        {
            try
            {
                await GeneratePdfInternal(inputPathFile, outputPathFile).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                LogError(ex.ToString());

                throw new WeasyPrintException(ex.Message, ex);
            }
        }

        public byte[] GeneratePdfFromUrl(string url)
        {
            byte[] result;

            try
            {
                result = GeneratePdfFromUrlInternal(url).GetAwaiter().GetResult();

                return result;

            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
                throw new WeasyPrintException(ex.Message, ex);
            }
        }

        public async Task<byte[]> GeneratePdfFromUrlAsync(string url)
        {
            byte[] result;

            try
            {
                result = await GeneratePdfFromUrlInternal(url);

                return result; 
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());

                throw new WeasyPrintException(ex.Message, ex);
            }
        }

        private async Task<byte[]> GeneratePdfInternal(string htmlText)
        {
            byte[] result;

            await EnsureFilesExisted()
                .ConfigureAwait(false);

            var data = Encoding.UTF8.GetBytes(htmlText);
            var fileName = $"{Guid.NewGuid().ToString().ToLower()}";

            var inputFileName = $"{fileName}.html";
            var outputFileName = $"{fileName}.pdf";

            var fullFilePath = await _fileManager.CreateFile(inputFileName, data)
                .ConfigureAwait(false);

            var cmd = $"/c python.exe scripts/weasyprint.exe \"{fullFilePath}\" \"{outputFileName}\" -e utf8";

            var workingDir = _fileManager.FolderPath;

            await _invoker.ExcuteAsync(workingDir, "cmd.exe", cmd)
                .ConfigureAwait(false);

            await _fileManager.Delete(fullFilePath)
                .ConfigureAwait(false);

            result = await _fileManager.ReadFile(outputFileName)
                .ConfigureAwait(false);

            await _fileManager.Delete(outputFileName)
                .ConfigureAwait(false);

            return result;
        }

        private async Task GeneratePdfInternal(string inputPathFile, string outputPathFile)
        {
            if (!File.Exists(inputPathFile))
                throw new FileNotFoundException();

            await EnsureFilesExisted()
                .ConfigureAwait(false);

            var cmd = $"/c python.exe scripts/weasyprint.exe \"{inputPathFile}\" \"{outputPathFile}\" -e utf8";

            var workingDir = _fileManager.FolderPath;

            await _invoker.ExcuteAsync(workingDir, "cmd.exe", cmd)
                .ConfigureAwait(false);
        }

        private async Task<byte[]> GeneratePdfFromUrlInternal(string url)
        {
            byte[] result;

            await EnsureFilesExisted()
                .ConfigureAwait(false);

            var fileName = $"{Guid.NewGuid().ToString().ToLower()}.pdf";

            var outputFileName = Path.Combine(_fileManager.FolderPath, $"{fileName}");

            var cmd = $"/c python.exe scripts/weasyprint.exe \"{url}\" \"{outputFileName}\" -e utf8";

            var workingDir = _fileManager.FolderPath;

            await _invoker.ExcuteAsync(workingDir, "cmd.exe", cmd)
                .ConfigureAwait(false);


            result = await _fileManager.ReadFile(fileName)
                .ConfigureAwait(false);

            await _fileManager.Delete(fileName)
                .ConfigureAwait(false);

            return result;
        }

        private async Task EnsureFilesExisted()
        {
            if (!_fileManager.IsFilesExsited())
                await _fileManager.InitFilesAsync();
        }

        private void LogOutput(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            OnDataOutput?.Invoke(new OutputEventArgs(data));

            _trace?.Info(data);
        }

        private void LogError(string data)
        {
            if (string.IsNullOrWhiteSpace(data))
                return;

            OnDataError?.Invoke(new OutputEventArgs(data));

            _trace?.Info($"Error: {data}");
        }

        public void Dispose()
        {
            _invoker.Dispose();
        }

        private void SetEnviromentVariables()
        {
            var variables = new Dictionary<string, string>()
            {
                ["PATH"] = "Scripts;gtk3;%PATH%",
            };

            _environmentVariables = variables;

        }
    }
}
