using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    class Class5
    {
        private const string ShukusenPath = @"..\Shukusen\ShukuSen.exe";

        public static void ExecuteShukusen(string work, string target)
        {
            using (var rwlock = new ReaderWriterLockSlim())
            {
                var targets = Chunk(Directory.GetFiles(target)).ToArray();
                //var locked = 3;
                //var index = 0;

                targets.AsParallel().ForAll(files =>
                {
                    var arg = string.Join(" ", files.Select(file => $"\"{file}\""));

                    StartProcess(work, Path.Combine(work, ShukusenPath), arg);

                    //if (locked < Interlocked.Increment(ref index))
                    //{
                    //    rwlock.EnterWriteLock();
                    //}

                    //var arg = string.Join(" ", files.Select(file => $"\"{file}\""));

                    //StartProcess(work, Path.Combine(work, ShukusenPath), arg);

                    //if (locked <= Interlocked.Decrement(ref index))
                    //{
                    //    rwlock.ExitWriteLock();
                    //}
                });

                targets.AsParallel().ForAll(files => files.AsParallel().ForAll(file =>
                {
                    // 縮小後ﾌｧｲﾙ名を作成
                    var dst = Path.Combine(
                        Path.GetDirectoryName(file),
                        $"s-{Program.GetFileNameWithoutExtension(file)}.jpg"
                    );

                    var fi = new FileInfo(dst);

                    if (fi.Exists && fi.Length != 0)
                    {
                        // 縮小が成功していたら元ﾌｧｲﾙを削除
                        File.Delete(file);
                    }
                    else
                    {
                        // 縮小が失敗していて、且つ、縮小後ﾌｧｲﾙが残っていたら後ﾌｧｲﾙを削除
                        if (fi.Exists) fi.Delete();

                        // 縮小前のﾌｧｲﾙをﾘﾈｰﾑ
                        File.Move(file, dst);
                    }
                }));
            }
        }

        private static IEnumerable<string[]> Chunk(IEnumerable<string> source)
        {
            var target = new List<string>();

            foreach (var file in source)
            {
                if (8000 < target.Sum(a => a.Length + 3))
                {
                    // ｺﾏﾝﾄﾞﾗｲﾝ引数の上限は8192文字なので、それを超えない範囲で配列を分割する。
                    yield return target.ToArray();

                    target.Clear();
                }

                target.Add(file);
            }

            if (target.Any())
            {
                yield return target.ToArray();
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
            var info = new ProcessStartInfo();

            info.WorkingDirectory = work;
            info.FileName = file;
            info.Arguments = argument;
            info.UseShellExecute = false;
            info.CreateNoWindow = true;
            info.ErrorDialog = true;
            info.RedirectStandardError = true;

            using (var process = Process.Start(info))
            {
                process.WaitForExit();
            }
        }

    }
}
