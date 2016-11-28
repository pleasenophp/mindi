using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace MinDI.Helpers
{
    // Internal helper for file operations
    public class FileHelper
    {
        public static IEnumerable<FileInfo> AllFilesIn(string path, bool recursively = false)
        {
            return new DirectoryInfo(path).GetFiles("*.*", recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        }

        public static IEnumerable<FileInfo> AllFilesInApplicationFolder()
        {
            var uri = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            return AllFilesIn(Path.GetDirectoryName(uri.LocalPath));
        }

    }
}