using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Balbarak.WeasyPrint
{
    public class WeasyPrintClient : IDisposable
    {
        private Process _nativeProccess;

        public WeasyPrintClient()
        {
            SetupGtkPath();
        }
        
        public void TestWeasy()
        {
            ExcuteCommand("weasyprint.exe ../index.html ../test.pdf");
            
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

        private void InitProccess()
        {
            KillProc();

            _nativeProccess = new Process();

            _nativeProccess.StartInfo.FileName = @"cmd.exe";
            
            _nativeProccess.StartInfo.EnvironmentVariables["PATH"] = "gtk3;%PATH%";

            _nativeProccess.StartInfo.WorkingDirectory = $"{Directory.GetCurrentDirectory()}\\lib";
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
            Debug.WriteLine($"Error: {e.Data}");
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            Debug.WriteLine(e.Data);
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
