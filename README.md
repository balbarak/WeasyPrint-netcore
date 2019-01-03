# Introduction
WeasyPrint Wrapper for .Net on Windows to generate pdf from html. It use [WeasyPrint](https://github.com/Kozea/WeasyPrint) to generate pdf from html without any extra installtion and setup on windows.

`Balbarak.Weasypont` simplify the using of WeasyPrint on Windows
# Getting started

## Installtion

From nuget packages

`PM> Install-Package Balbarak.WeasyPrint -Version 0.7.1`

## Usage

### From html text 

```C#
using Balbarak.WeasyPrint
using System.IO;

using (WeasyPrintClient client = new WeasyPrintClient())
{
    var html = "<!DOCTYPE html><html><body><h1>Hello World</h1></body></html>";

    var binaryPdf = client.GeneratePdf(html);

    File.WriteAllBytes("result.pdf",binaryPdf);
}
```

### From html file
```C#
using (WeasyPrintClient client = new WeasyPrintClient())
{
    var input = @"path\to\input.html";
    var output = @"path\to\output.pdf";

    client.GeneratePdf(input,output;
}
```

### Watch output and errors
```C#
using (WeasyPrintClient client = new WeasyPrintClient())
{
    var input = @"path\to\input.html";
    var output = @"path\to\output.pdf";

    client.OnDataError += OnDataError;
    client.OnDataOutput += OnDataOutput;

    client.GeneratePdf(input,output;
}

private void OnDataOutput(OutputEventArgs e)
{
    Console.WriteLine(args.Data);
}

private void OnDataError(OutputEventArgs e)
{
    Console.WriteLine(e.Data);
}
```

# Third Parties
* [WeasyPrint](https://github.com/Kozea/WeasyPrint) - BSD 3-Clause License 
* [Python 3.6 Embedded](https://wiki.python.org/moin/EmbeddedPython) - [License](https://docs.python.org/3/license.html)
* [Gtk3 for Windows](https://www.gtk.org/support.php)
