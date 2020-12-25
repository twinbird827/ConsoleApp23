using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    class Class1
    {
        public static string RenameDirectory(string value)
        {
            var dir = Path.GetDirectoryName(value);
            var src = Path.GetFileName(value);
            var dst = GetWithoutExtension(src.Trim(' ', '　', '.'));

            if (src == dst)
            {
                return value;
            }

            var result = Path.Combine(dir, GetUnique(dir, dst));
            Directory.Move(Path.Combine(dir, src), result);
            return result;
        }

        private static string GetWithoutExtension(string value)
        {
            var result = Path.GetFileNameWithoutExtension(value);
            if (result == value)
            {
                return result;
            }
            else
            {
                return GetWithoutExtension(result);
            }
        }

        private static string GetUnique(string dir, string name)
        {
            if (Directory.Exists(Path.Combine(dir, name)))
            {
                return GetUnique(dir, name, 1);
            }
            else
            {
                return name;
            }
        }

        private static string GetUnique(string dir, string name, int index)
        {
            var tmp = $"{name} ({index})";
            if (Directory.Exists(Path.Combine(dir, tmp)))
            {
                return GetUnique(dir, name, index + 1);
            }
            else
            {
                return tmp;
            }
        }

    }
}
