using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    /// <summary>
    /// 不要なﾌｧｲﾙ・ﾃﾞｨﾚｸﾄﾘを削除する。
    /// </summary>
    class Class3
    {
        /// <summary>
        /// 削除対象のﾌｧｲﾙ拡張子
        /// </summary>
        private static string[] IgnoreExtension = new string[]
        {
            ".db",
            ".dll",
            ".htm",
            ".lnk",
            ".url",
            ".html",
            ".shtml",
            ".txt"
        };

        /// <summary>
        /// 削除対象のﾃﾞｨﾚｸﾄﾘ名
        /// </summary>
        private static string[] IgnoreDirectory = new string[]
        {
            "単ページ"
        };

        public static void RemoveDirAndFile(string target)
        {
            if (IgnoreDirectory.Any(dir => dir == Path.GetFileName(target)))
            {
                Program.DirectoryDelete(target);
            }

            foreach (var file in Directory.GetFiles(target))
            {
                if (IgnoreExtension.Any(ext => file.ToLower().EndsWith(ext)))
                {
                    Program.FileDelete(file);
                }
            }

            foreach (var dir in Directory.GetDirectories(target))
            {
                RemoveDirAndFile(dir);
            }
        }
    }
}
