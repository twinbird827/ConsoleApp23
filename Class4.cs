using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    class Class4
    {
        private const string Unique = "!3!";

        public static void RenameAllFile(string target)
        {
            RenameAllDir(target, "", target);

            foreach (var dir in Directory.GetDirectories(target))
            {
                Program.DirectoryDelete(new DirectoryInfo(dir));
            }

            foreach (var r in Directory.GetFiles(target).Select((s, i) => new { s, i }).ToArray())
            {
                var file = r.s;
                var dstn = string.Format("{0,0:D5}", r.i);
                File.Move(file, Path.Combine(target, $"{dstn}{Path.GetExtension(file)}"));
            }
        }

        private static void RenameAllDir(string basedir, string identifier, string target)
        {
            identifier = $"{identifier}{Unique}";

            foreach (var r in Directory.GetFiles(target).Select((s, i) => new { s, i }).ToArray())
            {
                var file = new FileInfo(r.s);
                var dstn = string.Format("{0,0:D5}", r.i);
                file.MoveTo(Path.Combine(basedir, $"{identifier}{dstn}{Path.GetExtension(file.Extension)}"));
            }

            foreach (var r in Directory.GetDirectories(target).OrderBy(s => GetOrderBy(s)).Select((s, i) => new { s, i }).ToArray())
            {
                var src = r.s;
                var dir = $"{identifier}{$"{r.i}".PadLeft(3, '0')}";

                RenameAllDir(basedir, dir, src);
            }
        }

        private static string GetOrderBy(string value)
        {
            value = Regex.Replace(value, @"\d+", p => p.Value.PadLeft(6, '0'));
            value = value.Replace("cover", "!1!");
            value = value.Replace("color", "!2!");
            return value;
        }

    }
}
