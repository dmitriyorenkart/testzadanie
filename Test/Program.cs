using System;
using System.Diagnostics;

namespace Test
{
    class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("Тестовое задание от Кросс-Информ");
            Console.Write("Введите абсолютный путь к текстовому файлу: ");
            var filePath = Console.ReadLine();

            try
            {
                var myStopwatch = new Stopwatch();
                myStopwatch.Start();
                
                var result = TextAnalyser.GetTenMostFrequentTripletsFromFile(filePath);
                
                myStopwatch.Stop();
                Console.WriteLine("Результат операции: {0}", result);
                Console.WriteLine("Time = {0}", myStopwatch.ElapsedMilliseconds);
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}