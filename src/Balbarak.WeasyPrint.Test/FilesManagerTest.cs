using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Balbarak.WeasyPrint.Test
{
    public class FilesManagerTest
    {
        [Fact]
        public async Task Should_Init_Files()
        {
            FilesManager manager = new FilesManager();

            await manager.InitFilesAsync();

            var files = Directory.GetFiles(manager.FolderPath);

            var hasFiles = files.Length == 7;

            Assert.True(hasFiles);
        }
    }
}
