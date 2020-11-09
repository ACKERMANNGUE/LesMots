///file WordManager.cs
///author Ackermann Gawen CFPT-Informatique
///date 02.11.2020
///brief Class managing the different options related to a file containing words
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSL_LesMots
{
    class WordManager
    {
        private const string SAVE_PATH = @"C:\Users\admin\Desktop\LesMots\ordered_lorem.txt";
        private const string PATH = @"C:\Users\admin\Desktop\LesMots\";
        private const int NUMBER_OF_FILES = 100;

        private SortedDictionary<string, int> words;

        private int numberOfFiles;
        private long numberOfLineToReadInABlock;

        public int NumberOfFiles { get => numberOfFiles; }
        public long NumberOfLineToReadInABlock { get => numberOfLineToReadInABlock; }

        public WordManager()
        {
            numberOfFiles = NUMBER_OF_FILES;
            words = new SortedDictionary<string, int>();
        }

        /// <summary>
        /// Split a file into smaller parts
        /// </summary>
        /// <param name="pPath">The path of the file</param>
        public void SplitFile(string pPath)
        {
            Debug.Print("Split des fichiers");
            using (FileStream fileStream = File.OpenRead(pPath))
            {
                numberOfLineToReadInABlock = fileStream.Length / numberOfFiles;

                using (StreamReader sr = new StreamReader(fileStream, Encoding.UTF8, true, 4096))
                {
                    string wordsInFile;

                    for (int i = 0; i < numberOfFiles; i++)
                    {
                        using (FileStream fs = File.Create(PATH + i + ".txt"))
                        {
                            for (long j = 0; j < numberOfLineToReadInABlock; j++)
                            {
                                if ((wordsInFile = sr.ReadLine()) != null)
                                {
                                    UTF8Encoding encoding = new UTF8Encoding(true);
                                    fs.Write(encoding.GetBytes(wordsInFile), 0, encoding.GetByteCount(wordsInFile));
                                    j += encoding.GetByteCount(wordsInFile);
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Split words by a filter
        /// </summary>
        /// <param name="pPath">The path of the file</param>
        /// <returns>A dictionnary containing the splitted words</returns>
        public ConcurrentDictionary<string, int> SplitWords(int pPath)
        {
            Debug.Print("Split des mots");
            ConcurrentDictionary<string, int> tmpWords = new ConcurrentDictionary<string, int>();

            using (FileStream fileStream = File.OpenRead(PATH + pPath + ".txt"))
            {
                using (StreamReader sr = new StreamReader(fileStream, Encoding.UTF8, true, 4096))
                {
                    //read the entire file
                    string wordsInFile = sr.ReadToEnd();
                    //split the words
                    char[] filter = { ' ', ',', '.', '?', '!', '\r', '\n' };
                    string[] splittedWords = wordsInFile.Split(filter);
                    //parallelisation of the split
                    Parallel.For(0, splittedWords.Length, i =>
                    {
                        if (splittedWords[i].Length > 0)
                        {
                            if (!tmpWords.TryAdd(splittedWords[i], 1)){
                                tmpWords[splittedWords[i]]++;
                            }
                        }
                    });
                }
            }
            //Delete the actual file
            DeleteFile(pPath);
            return tmpWords;
        }

        /// <summary>
        /// Delete a file
        /// </summary>
        /// <param name="filename">The name of the file in int because they are generated in a loop from numberOfFiles</param>
        /// <returns>True if it has been deleted, false if it hasn't</returns>
        public bool DeleteFile(int filename)
        {
            Debug.Print("Suppression du fichier");
            bool result = false;
            try
            {
                File.Delete(PATH + filename + ".txt");
                result = true;
            }
            catch (Exception)
            {
                throw new Exception("Cannot delete the file named :" + filename + " in directory : " + PATH);
            }
            return result;
        }

        /// <summary>
        /// Display the number of words different and total
        /// </summary>
        /// <returns>The number of words different and total</returns>
        public override string ToString()
        {
            int nbWordsTotal = 0;
            foreach (KeyValuePair<string, int> entry in words)
            {
                nbWordsTotal += entry.Value;
            }
            return $"Nombre de mots différents : {words.Count} \t Nombre de mots totaux : {nbWordsTotal}";
        }

        /// <summary>
        /// Display the number of words and their occurences
        /// </summary>
        /// <returns>The words and their occurences</returns>
        public string DisplayWordsAndOccurences()
        {
            string output = "";
            foreach (KeyValuePair<string, int> word in words)
            {
                output += word.Key + "\t" + word.Value + "\n";
            }
            return output;

        }

        /// <summary>
        /// Write a sorted dictionnary into a file
        /// </summary>
        /// <param name="keys">The list of the keys</param>
        /// <param name="files">The datas that are stored in the files</param>
        public void WriteOrderedFile(List<string> keys, Dictionary<string, int> files)
        {
            Debug.Print("Écriture des mots dans le fichier");
            //sort of the keys
            keys.Sort();
            using (System.IO.StreamWriter file =
             new System.IO.StreamWriter(SAVE_PATH))
            {
                foreach (string key in keys)
                {
                    for (int i = 0; i < files[key]; i++)
                    {
                        file.WriteLine(key);
                    }
                }
            }
        }
    }
}
