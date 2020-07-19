using Balbarak.WeasyPrint.Resources;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Balbarak.WeasyPrint.Test")]

namespace Balbarak.WeasyPrint
{

    public class FilesManager
    {
        public string FolderPath { get; }

        public FilesManager()
        {
            FolderPath = GetFolderPath();
        }

        public void InitFiles()
        {
            if (!Directory.Exists(FolderPath))
            {
                Directory.CreateDirectory(FolderPath);
            }
            else
            {
                DeleteFiles();
                Directory.CreateDirectory(FolderPath);
            }

            var folderData = FileResx.win64_v51;

            var compressedFileName = Path.Combine(FolderPath, "data.zip");

            File.WriteAllBytes(compressedFileName, folderData);

            ZipFile.ExtractToDirectory(compressedFileName, FolderPath);

            File.Delete(compressedFileName);

        }

        public bool IsFilesExsited()
        {
            if (!Directory.Exists(FolderPath))
                return false;

            var files = Directory.GetFiles(FolderPath);

            var dirs = Directory.GetDirectories(FolderPath);

            var hasPython = files.Where(a => a.Contains("python.exe")).FirstOrDefault() != null;
            var hasScripts = dirs.Where(a => a.Contains("Scripts")).FirstOrDefault() != null;

            if (!hasScripts)
                return false;

            if (!hasPython)
                return false;


            return true;
        }

        private string GetFolderPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            var folderName = "balbarak-weasyprintv51";

            var fullPath = Path.Combine(appDataPath, folderName);

            return fullPath;
        }

        private void DeleteFiles()
        {
            if (!Directory.Exists(FolderPath))
                return;


            Directory.Delete(FolderPath, true);

        }
    }
}
