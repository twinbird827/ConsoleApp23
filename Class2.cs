using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp23
{
    class Class2
    {
        public static void OrganizeExtension(string baseDir, string target)
        {
            foreach (var file in Directory.GetFiles(target))
            {
                var src = Path.GetExtension(file).ToLower();
                var dst = GetExtension(file).ToLower();
                if (src != dst)
                {
                    File.Move(file, Path.Combine(Path.GetDirectoryName(file), $"{Path.GetFileNameWithoutExtension(file)}{dst}"));
                }
            }

            foreach (var dir in Directory.GetDirectories(target))
            {
                OrganizeExtension(baseDir, dir);
            }
        }

        private static string GetExtension(string value)
        {
            try
            {
                using (var bitmap = new Bitmap(value))
                {
                    var result = decoders.FirstOrDefault(ici => ici.FormatID == bitmap.RawFormat.Guid);
                    if (result != null)
                    {
                        return result.FilenameExtension.Split(';').First().Substring(1);
                    }
                }
            }
            catch
            {

            }
            return Path.GetExtension(value);
        }
        private static ImageCodecInfo[] decoders = ImageCodecInfo.GetImageDecoders();
    }
}
