using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    class Class6
    {
        public static void CreateZipFromDirectory(string src)
        {
            MyIO.FileDelete($"{src}.zip");
            CreateZipFromDirectory(src, $"{src}.zip");
            MyIO.DirectoryDelete(new DirectoryInfo(src));
        }

        private static void CreateZipFromDirectory(string src, string dst, CompressionLevel level = CompressionLevel.Optimal, bool includeBaseDirectory = true)
        {
            ZipFile.CreateFromDirectory(src, dst, level, includeBaseDirectory);
        }

    }
}
