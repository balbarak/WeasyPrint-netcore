using System.IO;
using System.Runtime.InteropServices;
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

            var hasFiles = files.Length == 7 || !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

            Assert.True(hasFiles);
        }
    }
}
