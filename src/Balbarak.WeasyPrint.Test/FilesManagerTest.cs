using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace Balbarak.WeasyPrint.Test
{
    public class FilesManagerTest
    {
        [Fact]
        public void Should_Init_Files()
        {
            FilesManager manager = new FilesManager();

            manager.InitFiles();

            var files = Directory.GetFiles(manager.FolderPath);

            var hasFiles = files.Length == 7;

            Assert.True(hasFiles);
        }
    }
}
