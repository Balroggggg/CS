using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CSLab1Entropy
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.Unicode;

            string path = @"D:\Education\CS\Lab1\";
            string textFile;
            int totalCountSymbols;
            long fileSize;
            double entropy, infoQuant;
            Dictionary<char, double> symb = new Dictionary<char, double>();


            try
            {
                    Console.Write("Назва .txt файлу:");
                    textFile = Console.ReadLine();
                    Console.WriteLine();
                    path += textFile + ".txt";
                    fileSize = ReadFile(path, symb, out totalCountSymbols);
                    SymbolFrequency(symb, totalCountSymbols);
                    entropy = AvgEntropy(symb);
                    infoQuant = InfoQuantity(entropy, totalCountSymbols);
                    Print(symb, totalCountSymbols, entropy, infoQuant, fileSize);
            }
            catch (FileNotFoundException fnfexc)
            {
                Console.WriteLine(fnfexc.Message);
            }
            catch (IOException ioexc)
            {
                Console.WriteLine(ioexc.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }



        public static void SymbolFrequency(Dictionary<char, double> symb, int totalCount)
        {
            //The number of keys in the dictionary
            int countKeysDict = symb.Keys.Count;
            char[] keysDict = new char[countKeysDict];
            symb.Keys.CopyTo(keysDict, 0);

            for (int iter = 0; iter < countKeysDict; iter++)
            {
                symb[keysDict[iter]] /= totalCount;
            }
        }

        public static double InfoQuantity(double entrp, int symbCount)
        {
            return entrp * symbCount;
        }

        public static double AvgEntropy(Dictionary<char, double> symb)
        {
            int countsymb = symb.Keys.Count;
            char[] keysDict = new char[countsymb];
            symb.Keys.CopyTo(keysDict, 0);
            double probability = 0, entropy = 0;

            for (int iter = 0; iter < countsymb; iter++)
            {
                probability = symb[keysDict[iter]];
                entropy -= probability * Math.Log(probability, 2);
            }
            return entropy;
        }

        public static void Print(Dictionary<char, double> countsymb, int totalCount, double entropy, double infoQuant, long fileSize)
        {

            Console.WriteLine("Розмір файлу = {0} bytes", fileSize);
            Console.WriteLine("Кількість інформації в байтах = {0} bytes", infoQuant / 8);
            Console.WriteLine("Кількість інформації в бітах = {0} bits", infoQuant);
            Console.WriteLine("Ентропія = {0}", entropy);
            Console.WriteLine("Кількість символів = {0}", totalCount);


            SortedDictionary<char, double> sortedDict = new SortedDictionary<char, double>(countsymb);
            foreach (KeyValuePair<char, double> kvp in sortedDict)
            {
                Console.WriteLine(kvp.Key + " - " + kvp.Value);
            }
        }

        public static long ReadFile(string pathFile, Dictionary<char, double> symb, out int totalCountSymb)
        {
            FileInfo fileSize = new FileInfo(pathFile);
            int i;

            double symbRec;
            totalCountSymb = 0;

            string allText = File.ReadAllText(pathFile);
            i = 0;
            while (i < allText.Length)
            {
                symbRec = 1;
                if (!symb.ContainsKey(allText[i]))
                {
                    symb.Add(allText[i], symbRec);
                }
                else
                    if (symb.ContainsKey(allText[i]))
                {
                    symb[allText[i]]++;
                }
                i++;
                totalCountSymb++;
            }
            return fileSize.Length;
        }

    }
}
