using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Balbarak.WeasyPrint.Internals
{
    internal partial class ProcessInvoker
    {
        [DllImport("libc", SetLastError = true)]
        private static extern int kill(int pid, int sig);

        private void NixKillProcessTree()
        {
            try
            {
                if (_proc?.HasExited == false)
                {
                    _proc?.Kill();
                }
            }
            catch (InvalidOperationException ex)
            {

                //Trace.Info("Ignore InvalidOperationException during Process.Kill().");
                //Trace.Info(ex.ToString());
            }
        }

        private async Task<bool> SendPosixSignal(PosixSignals signal, TimeSpan timeout)
        {
            //Trace.Info($"Sending {signal} to process {_proc.Id}.");
            int errorCode = kill(_proc.Id, (int)signal);
            if (errorCode != 0)
            {
                //Trace.Info($"{signal} signal doesn't fire successfully.");
                //Trace.Info($"Error code: {errorCode}.");
                return false;
            }

            //Trace.Info($"Successfully send {signal} to process {_proc.Id}.");
            //Trace.Info($"Waiting for process exit or {timeout.TotalSeconds} seconds after {signal} signal fired.");
            var completedTask = await Task.WhenAny(Task.Delay(timeout), _processExitedCompletionSource.Task);
            if (completedTask == _processExitedCompletionSource.Task)
            {
                //Trace.Info("Process exit successfully.");
                return true;
            }
            else
            {
                //Trace.Info($"Process did not honor {signal} signal within {timeout.TotalSeconds} seconds.");
                return false;
            }
        }

    }
}
