﻿using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace ConsoleApp23
{
    /// <summary>
    /// 拡張子を正しいものにする。
    /// </summary>
    class Class2
    {
        public static void OrganizeExtension(string baseDir, string target)
        {
            foreach (var file in Directory.GetFiles(target))
            {
                var src = Path.GetExtension(file).ToLower();
                var dst = GetExtension(file)?.ToLower();
                if (dst == null)
                {
                    MyIO.FileDelete(file);
                }
                else if (src != dst)
                {
                    MyIO.FileMove(file, Path.Combine(Path.GetDirectoryName(file), $"{Program.GetFileNameWithoutExtension(file)}{dst}"));
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
            return null;
        }
        private static ImageCodecInfo[] decoders = ImageCodecInfo.GetImageDecoders();
    }
}
