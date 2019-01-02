using Balbarak.WeasyPrint.Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Balbarak.WeasyPrint
{
    public class WeasyPrintClient : IDisposable
    {
        public event EventHandler OnDataOutput;
        public event EventHandler OnDataError;

        private readonly string _libDir = Path.Combine(Directory.GetCurrentDirectory(), "weasyprint-files");

        private Process _nativeProccess;

        public WeasyPrintClient()
        {
            if (!CheckFiles())
                InitFiles();
        }
        
        public byte[] GeneratePdf(string htmlText)
        {
            byte[] result = null;

            try
            {
                var fileName = $"{Guid.NewGuid().ToString().ToLower()}";
                var dirSeparator = Path.DirectorySeparatorChar;
                
                var inputFileName = $"{fileName}.html";
                var outputFileName = $"{fileName}.pdf";

                var inputFullName = Path.Combine(_libDir,inputFileName);
                var outputFullName = Path.Combine(_libDir,outputFileName);

                File.WriteAllText(Path.Combine(_libDir,inputFileName), htmlText);

                ExcuteCommand($"python.exe weasyprint.exe {inputFileName} {outputFileName} -e utf8");

                result = File.ReadAllBytes(outputFullName);
                
                if (File.Exists(inputFullName))
                    File.Delete(inputFullName);

                if (File.Exists(outputFullName))
                    File.Delete(outputFullName);
            }
            catch (Exception ex)
            {
                OnDataError?.Invoke(this, new OutputEventArgs(ex.ToString()));
            }

            return result;
        }

        public void GeneratePdf(string inputPathFile,string outputPathFile)
        {
            try
            {
                ExcuteCommand($"python.exe weasyprint.exe {inputPathFile} {outputPathFile} -e utf8");
            }
            catch (Exception ex)
            {
                OnDataError?.Invoke(this, new OutputEventArgs(ex.ToString()));
            }
   
        }

        private void ExcuteCommand(string cmd)
        {
            InitProccess();

            _nativeProccess.StartInfo.Arguments = $@"/c {cmd}";

            _nativeProccess.Start();

            _nativeProccess.BeginOutputReadLine();
            _nativeProccess.BeginErrorReadLine();

            _nativeProccess.WaitForExit();

        }

        private bool CheckFiles()
        {
            LogOutput("Checking files ...");

            if (!Directory.Exists(_libDir))
                return false;

            var files = Directory.GetFiles(_libDir);

            if (files.Count() < 22)
                return false;

            var containPython = files.Where(a => a.Contains("python.exe")).FirstOrDefault() != null;

            if (!containPython)
                return false;

            return true;
        }

        private void InitFiles()
        {
            LogOutput($"Checking {_libDir} direcoty");

            if (!Directory.Exists(_libDir))
            {
                LogOutput("Creating direcotry");

                Directory.CreateDirectory(_libDir);
            }
            else
            {
                LogOutput("Deleting corrupted files ...");

                Directory.Delete(_libDir,true);

                Directory.CreateDirectory(_libDir);
            }

            var filesData = FileResx.libCompress;

            var zipFileName = Path.Combine(_libDir, "weasyFile.zip");

            File.WriteAllBytes(zipFileName, filesData);

            LogOutput("Extracting files ...");

            ZipFile.ExtractToDirectory(zipFileName, _libDir);

            LogOutput($"Deleting {zipFileName}");

            File.Delete(zipFileName);
        }

        private void InitProccess()
        {
            KillProc();

            var workingDir = _libDir;

            _nativeProccess = new Process();

            _nativeProccess.StartInfo.FileName = @"cmd.exe";

            _nativeProccess.StartInfo.EnvironmentVariables["PATH"] = "gtk3;%PATH%";

            _nativeProccess.StartInfo.EnvironmentVariables["FONTCONFIG_FILE"] = $"{workingDir}\\gtk3\\fonts.config";

            _nativeProccess.StartInfo.WorkingDirectory = workingDir;
            _nativeProccess.StartInfo.UseShellExecute = false;
            _nativeProccess.StartInfo.RedirectStandardInput = true;
            _nativeProccess.StartInfo.RedirectStandardOutput = true;
            _nativeProccess.StartInfo.RedirectStandardError = true;
            _nativeProccess.StartInfo.CreateNoWindow = true;

            _nativeProccess.OutputDataReceived += OnOutputDataReceived;
            _nativeProccess.ErrorDataReceived += OnErrorDataReceived;
            _nativeProccess.Exited += OnExited;

        }

        private void OnExited(object sender, EventArgs e)
        {
            Debug.WriteLine("Proccess exited");
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogError(e.Data);

            Debug.WriteLine($"Error: {e.Data}");
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            LogOutput(e.Data);

            Debug.WriteLine(e.Data);
        }

        private void LogOutput(string data)
        {
            OnDataOutput?.Invoke(this, new OutputEventArgs(data));
        }

        private void LogError(string data)
        {
            OnDataError?.Invoke(this, new OutputEventArgs(data));
        }

        public void Dispose()
        {
            KillProc();
        }

        private void KillProc()
        {
            if (_nativeProccess != null)
            {
                try
                {
                    _nativeProccess.Kill();
                }
                catch
                {

                }

                _nativeProccess.Dispose();
            }
        }
    }
}
