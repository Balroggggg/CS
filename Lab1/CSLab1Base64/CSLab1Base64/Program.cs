using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CSLab1Base64
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.OutputEncoding = System.Text.Encoding.Unicode;

            string path = @"D:\Education\CS\Lab1\";
            string pathToWrite = @"D:\Education\CS\Lab1\EncryptedText\";
            string pathToCmprssed = @"D:\Education\CS\Lab1\EncryptedText\";
            string textFile, encryptFile, compressedFile;
            char[] encodeText;
            int totalCountCharacters;
            long fileSize;
            double entropy, infoQuant;
            Dictionary<char, double> symblos = new Dictionary<char, double>();
            ToBase64 base64;

            try
            {
                Console.Write("Назва .txt файлу: ");
                textFile = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Назва .txt файлу для кодуваня у Base64:");
                encryptFile = Console.ReadLine();
                Console.WriteLine();

                Console.Write("Назва файлу для Base64 стиснення: ");
                compressedFile = Console.ReadLine();
                Console.WriteLine();

                path += textFile + ".txt";
                pathToWrite += encryptFile + ".txt";

                fileSize = ReadFile(path, symblos, out totalCountCharacters);
                SymbolFrequency(symblos, totalCountCharacters);
                entropy = AvgEntropy(symblos);
                infoQuant = InfoQuantity(entropy, totalCountCharacters);
                Print(infoQuant, fileSize);


                fileSize = ReadFile(pathToWrite, symblos, out totalCountCharacters);
                SymbolFrequency(symblos, totalCountCharacters);
                entropy = AvgEntropy(symblos);
                infoQuant = InfoQuantity(entropy, totalCountCharacters);

                base64 = new ToBase64(GetTextInBytes(path));
                encodeText = base64.GetEncoded();

                string encryptRow = new string(encodeText);
                WriteFile(pathToWrite, encryptRow);
                Print(infoQuant, fileSize, CheckEncoding(path));

                pathToCmprssed += compressedFile + ".txt.bz2";
                fileSize = ReadFile(pathToCmprssed, symblos, out totalCountCharacters);
                SymbolFrequency(symblos, totalCountCharacters);
                entropy = AvgEntropy(symblos);
                infoQuant = InfoQuantity(entropy, totalCountCharacters);
                Print(infoQuant, fileSize);
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

        public static byte[] GetTextInBytes(string path)
        {
            Encoding utf8 = Encoding.UTF8;

            string allText = File.ReadAllText(path);
            char[] symbols = allText.ToCharArray(0, allText.Length);
            byte[] bytes = utf8.GetBytes(symbols);

            return bytes;
        }

        public static string CheckEncoding(string path)
        {
            string allText = File.ReadAllText(path);
            string base64Text = Convert.ToBase64String(Encoding.UTF8.GetBytes(allText));

            return base64Text;
        }

        public static void Print(double infoQuant, long fileSize)
        {
            Console.WriteLine("Розмір файлу = {0} bytes", fileSize);
            Console.WriteLine("Кількість інформації в байтах = {0} bytes", infoQuant / 8);
            Console.WriteLine("Кількість інформації в бітах = {0} bits\n", infoQuant);
        }

        public static void Print(double infoQuant, long fileSize, string base64Text)
        {
            Console.WriteLine("Розмір файлу = {0} bytes", fileSize);
            Console.WriteLine("Кількість інформації в байтах = {0} bytes", infoQuant / 8);
            Console.WriteLine("Кількість інформації в бітах = {0} bits\n", infoQuant);
            Console.WriteLine();
            Console.WriteLine(base64Text);
            Console.WriteLine();
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

        public static void WriteFile(string path, string text)
        {
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.WriteLine(text);
            }
        }

     

    }
}
