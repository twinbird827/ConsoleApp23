using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    class Class5
    {
        private const string ShukusenPath = @"..\Shukusen\ShukuSen.exe";

        public static void ExecuteShukusen(string work, string target)
        {
            foreach (var files in Chunk(Directory.GetFiles(target)))
            {
                var arg = string.Join(" ", files.Select(file => $"\"{file}\""));

                // 縮小専用を実行
                StartProcess(work, Path.Combine(work, ShukusenPath), arg);

                files.AsParallel().ForAll(src =>
                {
                    // 縮小後ﾌｧｲﾙ名を作成
                    var dst = Path.Combine(
                        Path.GetDirectoryName(src),
                        $"s-{Path.GetFileNameWithoutExtension(src)}.jpg"
                    );

                    var fi = new FileInfo(dst);

                    if (fi.Exists && fi.Length != 0)
                    {
                        // 縮小が成功していたら元ﾌｧｲﾙを削除
                        File.Delete(src);
                    }
                    else
                    {
                        // 縮小が失敗していて、且つ、縮小後ﾌｧｲﾙが残っていたら後ﾌｧｲﾙを削除
                        if (fi.Exists) fi.Delete();

                        // 縮小前のﾌｧｲﾙをﾘﾈｰﾑ
                        File.Move(src, dst);
                    }
                });
            }
        }

        private static IEnumerable<IEnumerable<string>> Chunk(IEnumerable<string> source)
        {
            var target = new List<string>();

            foreach (var file in source)
            {
                if (8000 < target.Sum(a => a.Length + 3))
                {
                    // ｺﾏﾝﾄﾞﾗｲﾝ引数の上限は8192文字なので、それを超えない範囲で配列を分割する。
                    yield return target;

                    target.Clear();
                }

                target.Add(file);
            }

            if (target.Any())
            {
                yield return target;
            }
        }

        /// <summary>
        /// ﾌﾟﾛｾｽを実行します。
        /// </summary>
        /// <param name="work">作業ﾃﾞｨﾚｸﾄﾘ</param>
        /// <param name="file">実行するﾌﾟﾛｾｽのﾊﾟｽ</param>
        /// <param name="argument">ﾌﾟﾛｾｽに渡す実行引数</param>
        private static void StartProcess(string work, string file, string argument)
        {
            var process = new ProcessStartInfo();

            process.WorkingDirectory = work;
            process.FileName = file;
            process.Arguments = argument;
            process.UseShellExecute = false;
            process.CreateNoWindow = true;
            process.ErrorDialog = true;
            process.RedirectStandardError = true;

            Process.Start(process).WaitForExit();
        }

    }
}
