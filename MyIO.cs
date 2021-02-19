using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    public static class MyIO
    {
        [DllImport("kernel32.dll")]
        private static extern int GetShortPathName(string longPath, StringBuilder shortPathBuffer, int bufferSize);

        private static string GetShortPathName(string longpath)
        {
            const int bufferSize = 260;
            var sb = new StringBuilder(bufferSize);

            GetShortPathName(longpath, sb, bufferSize);

            return sb.ToString();
        }

        public static void FileDelete(string file)
        {
            FileDelete(new FileInfo(GetShortPathName(file)));
        }

        public static void FileDelete(FileInfo info)
        {
            if (!info.Exists)
            {
                return;
            }

            info.Attributes = FileAttributes.Normal;
            info.Delete();
        }

        public static void FileMove(string src, string dst)
        {
            File.Move(GetShortPathName(src), GetShortPathName(dst));
        }

        public static void DirectoryDelete(DirectoryInfo info)
        {
            if (!info.Exists)
            {
                return;
            }

            // ﾃﾞｨﾚｸﾄﾘ内のﾌｧｲﾙ、またはﾃﾞｨﾚｸﾄﾘを削除可能な属性にする。
            foreach (var file in info.GetFileSystemInfos("*", SearchOption.AllDirectories))
            {
                if (file.Attributes.HasFlag(FileAttributes.Directory))
                {
                    file.Attributes = FileAttributes.Directory;
                }
                else
                {
                    file.Attributes = FileAttributes.Normal;
                }
            }

            // ﾃﾞｨﾚｸﾄﾘの削除
            info.Delete(true);
        }

        public static void DirectoryDelete(string dir)
        {
            DirectoryDelete(new DirectoryInfo(dir));
        }


    }
}
