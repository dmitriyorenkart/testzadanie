using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Test
{
    public static class TextAnalyser
    {
        private const int TripletLength = 3;

        public static string GetTenMostFrequentTripletsFromFile(string filePath)
        {
            if (!(File.Exists(filePath) && Path.GetExtension(filePath) == ".txt"))
            {
                throw new Exception("не удалось найти текстовый файл");
            }
            
            var allOccurrencesTriplets = GetAllOccurrencesTripletsFromFile(filePath);

            var resultList = GetNFirstMaxByValuePairs(allOccurrencesTriplets, 10);

            //return string.Join(", ", resultList.Select(pair => pair.Key + " = " + pair.Value.ToString()));
            return string.Join(", ", resultList.Select(pair => pair.Key));
        }
        
        private static ConcurrentDictionary<string, int> GetAllOccurrencesTripletsFromFile(string filePath)
        {
            var allOccurrencesTriplets = new ConcurrentDictionary<string, int>();
            
            Parallel.ForEach(File.ReadLines(filePath), line =>
            {
                FindAndAddOccurrencesTripletsFromLine(allOccurrencesTriplets, line);
            });

            return allOccurrencesTriplets;
        }

        private static List<KeyValuePair<string, int>> GetNFirstMaxByValuePairs(IDictionary<string, int> dict, int numberFirstPairs)
        {
            var list = dict.ToList();
            
            list.Sort((pair1, pair2) => (-1)*pair1.Value.CompareTo(pair2.Value));
            
            return list.GetRange(0, list.Count < numberFirstPairs ? list.Count : numberFirstPairs);
        }

        private static void FindAndAddOccurrencesTripletsFromLine(
            ConcurrentDictionary<string, int> allOccurrencesTriplets,
            string line)
        {
            for (var i = 0; i < line.Length - TripletLength + 1; i++)
            {
                int leastNotLetterIndex;
                if (SubstringHasOnlyLetter(line, i, TripletLength, out leastNotLetterIndex))
                {
                    var substring = line.Substring(i, TripletLength).ToLower();

                    allOccurrencesTriplets.AddOrUpdate(substring, 1, (key, oldValue) => oldValue + 1);
                }
                else
                {
                    i = leastNotLetterIndex;
                }
            }
        }
        
        private static bool SubstringHasOnlyLetter(string line, int startIndex, int lengthSubString, out int leastNotLetterIndex)
        {
            leastNotLetterIndex = -1;
            
            for (var i = startIndex + lengthSubString - 1; i >= startIndex; i--)
            {
                if (!char.IsLetter(line[i]))
                {
                    leastNotLetterIndex = i;
                    break;
                }
            }

            return leastNotLetterIndex == -1;
        }
    }
}
