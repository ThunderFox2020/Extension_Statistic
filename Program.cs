using System;
using System.IO;
using System.Linq;

namespace Extension_Statistics
{
    public class Program
    {
        public static void Main()
        {
            Console.Write("Путь к директории: ");
            string path = Console.ReadLine()!;

            Console.Write("Показать файлы (+): ");
            bool showFiles = Console.ReadLine() == "+";

            Console.WriteLine();

            var query = from file in (new DirectoryInfo(path)).GetFiles("*", SearchOption.AllDirectories)
                        group file by Path.GetExtension(file.FullName) into g
                        orderby g.Key
                        select new
                        {
                            Extension = g.Key,
                            Count = g.Count(),
                            Size = (from file in g select file.Length).Sum(),
                            Files = g.OrderBy(file => file.FullName)
                        };

            int allCount = (from item in query select item.Count).Sum();
            long allSize = (from item in query select item.Size).Sum();

            foreach (var item1 in query)
            {
                Console.WriteLine($"Расширение: {item1.Extension}; Количество файлов: {item1.Count} ({((double)item1.Count / (double)allCount):P2}); Размер файлов: {(double)item1.Size / 1024d / 1024d:F2} МБ ({((double)item1.Size / (double)allSize):P2})");
                if (showFiles)
                {
                    foreach (var item2 in item1.Files)
                    {
                        Console.WriteLine($"- {item2.FullName.Replace(path, "root")}");
                    }
                    Console.WriteLine();
                }
            }

            if (!showFiles)
            {
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}