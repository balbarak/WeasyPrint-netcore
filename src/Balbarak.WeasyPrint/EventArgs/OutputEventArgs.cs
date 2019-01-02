using System;
using System.Collections.Generic;
using System.Text;

namespace Balbarak.WeasyPrint
{
    public class OutputEventArgs : EventArgs
    {
        public string Data { get; set; }

        public OutputEventArgs()
        {

        }

        public OutputEventArgs(string data)
        {
            this.Data = data;
        }
    }
}
