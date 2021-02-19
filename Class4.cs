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
        public static void RenameAllFile(string target)
        {
            int index = 0;
            var dirs = RenameAllDir(target).ToArray();
            var targets = new[] { target }.Concat(dirs);

            foreach (var dir in targets)
            foreach (var file in Directory.GetFiles(dir).OrderBy(s => GetOrderBy(s)).ToArray())
            {
                var name = string.Format("{0,0:D5}", ++index);
                var exte = Path.GetExtension(file);
                File.Move(file, Path.Combine(target, $"{name}{exte}"));
            }

            foreach (var dir in dirs)
            {
                MyIO.DirectoryDelete(dir);
            }
        }

        private static IEnumerable<string> RenameAllDir(string target)
        {
            foreach (var dir in Directory.GetDirectories(target).OrderBy(s => GetOrderBy(s)))
            {
                yield return dir;
                foreach (var child in RenameAllDir(dir))
                {
                    yield return child;
                }
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
