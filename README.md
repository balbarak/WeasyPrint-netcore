
![Nuget](https://img.shields.io/nuget/v/Balbarak.WeasyPrint)

# Introduction
WeasyPrint Wrapper for .Net on Windows to generate pdf from html. It uses [WeasyPrint](https://github.com/Kozea/WeasyPrint) to generate pdf from html without any extra installation and setup on Windows. 
For usage in not Windows environment we assume the `weasyprint` command is in the `PATH` and available in the commandline.

`Balbarak.WeasyPrint` simplifies the using of WeasyPrint on Windows
# Getting started

## Installation

From nuget packages

![Nuget](https://img.shields.io/nuget/v/Balbarak.WeasyPrint)

`PM> Install-Package Balbarak.WeasyPrint`



## Usage

### From html text 

```C#
using Balbarak.WeasyPrint
using System.IO;

using (WeasyPrintClient client = new WeasyPrintClient())
{
    var html = "<!DOCTYPE html><html><body><h1>Hello World</h1></body></html>";

    var binaryPdf = await client.GeneratePdfAsync(html);

    File.WriteAllBytes("result.pdf",binaryPdf);
}
```

### From html file
```C#
using (WeasyPrintClient client = new WeasyPrintClient())
{
    var input = @"path\to\input.html";
    var output = @"path\to\output.pdf";

    await client.GeneratePdfAsync(input,output);
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

    await client.GeneratePdfAsync(input,output);
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
