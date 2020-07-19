using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Balbarak.WeasyPrint.Internals
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PROCESS_BASIC_INFORMATION
    {
        public long ExitStatus;
        public long PebBaseAddress;
        public long AffinityMask;
        public long BasePriority;
        public long UniqueProcessId;
        public long InheritedFromUniqueProcessId;
    };

    internal enum PosixSignals : int
    {
        SIGINT = 2,
        SIGTERM = 15
    }

    internal class ProcessTerminationInfo
    {
        public ProcessTerminationInfo(int pid, bool expanded)
        {
            Pid = pid;
            ChildPidExpanded = expanded;
        }

        public int Pid { get; }
        public bool ChildPidExpanded { get; }
    }

    internal enum ConsoleCtrlEvent
    {
        CTRL_C = 0,
        CTRL_BREAK = 1
    }

    internal enum PROCESSINFOCLASS : int
    {
        ProcessBasicInformation = 0
    };

    internal class AsyncManualResetEvent
    {
        private volatile TaskCompletionSource<bool> m_tcs = new TaskCompletionSource<bool>();

        public Task WaitAsync() { return m_tcs.Task; }

        public void Set()
        {
            var tcs = m_tcs;
            Task.Factory.StartNew(s => ((TaskCompletionSource<bool>)s).TrySetResult(true),
                tcs, CancellationToken.None, TaskCreationOptions.PreferFairness, TaskScheduler.Default);
            tcs.Task.Wait();
        }

        public void Reset()
        {
            while (true)
            {
                var tcs = m_tcs;
                if (!tcs.Task.IsCompleted ||
                    Interlocked.CompareExchange(ref m_tcs, new TaskCompletionSource<bool>(), tcs) == tcs)
                    return;
            }
        }
    }
}
