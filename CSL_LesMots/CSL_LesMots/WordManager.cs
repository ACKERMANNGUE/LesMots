using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSL_LesMots
{
    class WordManager
    {
        private const string SAVE_PATH = @"C:\Users\Administrateur\Desktop\LesMots\ordered_lorem5.txt";

        SortedDictionary<string, int> words;

        public WordManager()
        {
            words = new SortedDictionary<string, int>();
        }

        public void LoadWords(string pPath)
        {
            //string[] tmp = new string[MAX_SIZE];
            using (StreamReader sr = File.OpenText(pPath))
            {
                string wordsInFile;
                while ((wordsInFile = sr.ReadLine()) != null)
                {
                    char[] filter = { ' ', ',', '.', '?', '!', '\r', '\n' };
                    string[] splittedWords = wordsInFile.Split(filter);
                    for (int i = 0; i < splittedWords.Length; i++)
                    {
                        if (splittedWords[i].Length > 0)
                        {
                            if (!words.ContainsKey(splittedWords[i]))
                            {
                                words.Add(splittedWords[i], 1);
                            }
                            else if (words.ContainsKey(splittedWords[i]))
                            {
                                words[splittedWords[i]] += 1;
                            }
                        }
                    }
                    wordsInFile = "";
                }
            }
        }

        public override string ToString()
        {
            int nbWordsTotal = 0;
            foreach (KeyValuePair<string, int> entry in words)
            {
                nbWordsTotal += entry.Value;
            }
            return $"Nombre de mots différents : {words.Count} \t Nombre de mots totaux : {nbWordsTotal}";
        }

        public string DisplayWordsAndOccurences() {
            string output = "";
            foreach (KeyValuePair<string, int> word in words)
            {
                output += word.Key + "\t" + word.Value + "\n";
            }
            return output;

        }

        public void WriteOrderedFile()
        {
            using (System.IO.StreamWriter file =
             new System.IO.StreamWriter(SAVE_PATH))
            {
                foreach (KeyValuePair<string, int> word in words)
                {
                    for (int i = 0; i < word.Value; i++)
                    {
                        file.WriteLine(word.Key);
                    }
                }
            }
        }
    }
}
