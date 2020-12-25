using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp23
{
    class Program
    {
        static void Main(string[] args)
        {
            // 作業ﾃﾞｨﾚｸﾄﾘを取得
            var work = System.AppDomain.CurrentDomain.BaseDirectory.TrimEnd('\\');

            // ｵﾌﾟｼｮﾝ
            var isExeShukusen = true;

            // ｵﾌﾟｼｮﾝを選択
            Console.WriteLine("起動ｵﾌﾟｼｮﾝを選択してください。");
            Console.WriteLine("0: 全て実行する。(ﾃﾞﾌｫﾙﾄ)");
            Console.WriteLine("1: 画像縮小をｽｷｯﾌﾟする。");

            // ｵﾌﾟｼｮﾝ読取&判定
            if (!CheckOptions(Console.ReadLine(), ref isExeShukusen))
            {
                return;
            }

            try
            {
                // 実行ﾊﾟﾗﾒｰﾀに対して処理実行
                args.AsParallel()
                    .ForAll(arg => Execute(work, arg, isExeShukusen));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadLine();
            }
        }

        /// <summary>
        /// 入力されたｵﾌﾟｼｮﾝの判定を行います。
        /// </summary>
        /// <param name="o">Console.ReadLine</param>
        /// <param name="isExeShukusen">縮小専用を実行するかどうか(参照)</param>
        /// <returns>true: OK / false: NG</returns>
        private static bool CheckOptions(string o, ref bool isExeShukusen)
        {
            switch (o)
            {
                case "":
                case "0":
                    // 全て実行する
                    // (空文字ならﾃﾞﾌｫﾙﾄ)
                    isExeShukusen = true;
                    return true;
                case "1":
                    // 画像縮小をｽｷｯﾌﾟする。
                    isExeShukusen = false;
                    return true;
                default:
                    // ｴﾗｰ
                    Console.WriteLine("認識できないｵﾌﾟｼｮﾝが指定されたのでｱﾌﾟﾘｹｰｼｮﾝを終了します。");
                    return false;
            }
        }

        /// <summary>
        /// 実行ﾊﾟﾗﾒｰﾀに対して処理を実行します。
        /// </summary>
        /// <param name="work">作業ﾃﾞｨﾚｸﾄﾘ</param>
        /// <param name="target">処理対象ﾌｧｲﾙ(ﾌｫﾙﾀﾞ)</param>
        /// <param name="isExeShukusen">縮小専用を実行するかどうか</param>
        private static void Execute(string work, string target, bool isExeShukusen)
        {
            Console.WriteLine($"target:{target}");

            try
            {
                if (!Directory.Exists(target))
                {
                    Console.WriteLine($"* ﾌｫﾙﾀﾞ以外のﾊﾟｽはｽｷｯﾌﾟします。");
                    return;
                }

                Console.WriteLine($"* ﾌｫﾙﾀﾞ名を整形します");
                target = Class1.RenameDirectory(target);

                Console.WriteLine($"* ﾌｧｲﾙの拡張子を正します");
                Class2.OrganizeExtension(target, target);

                Console.WriteLine($"* 不要なﾌｫﾙﾀﾞやﾌｧｲﾙを削除します");
                Class3.RemoveDirAndFile(target);

                Console.WriteLine($"* ﾌｧｲﾙ名を整形します");
                Class4.RenameAllFile(target);

                if (isExeShukusen)
                {
                    Console.WriteLine($"* ﾌｧｲﾙ内の画像を縮小します");
                    Class5.ExecuteShukusen(work, target);
                }

                Console.WriteLine($"* 圧縮処理を開始します");
                Class6.CreateZipFromDirectory(target);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.WriteLine("ｴﾗｰが発生したので該当ﾌｫﾙﾀﾞの処理を中断して続行します。");
            }
        }

        public static void DirectoryDelete(DirectoryInfo info)
        {
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

    }
}
