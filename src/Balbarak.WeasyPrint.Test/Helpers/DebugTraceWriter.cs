using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Balbarak.WeasyPrint.Test.Helpers
{
    public class DebugTraceWriter : ITraceWriter
    {
        public void Info(string message)
        {
            Debug.WriteLine(message);
        }

        public void Verbose(string message)
        {
            
        }
    }
}
