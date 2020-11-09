///file Program.cs
///author Ackermann Gawen CFPT-Informatique
///date 02.11.2020
///brief The program
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
    class Program
    {
        /* ! CHANGER LE PATH !*/
        //Fichier de texte très petit
        //const string PATH_INITIAL_FILE = @"C:\Users\admin\Desktop\LesMots\lorem5_mini.txt";
        //Fichier de texte moyen
        //const string PATH_INITIAL_FILE = @"C:\Users\admin\Desktop\LesMots\lorem5_moyen.txt";
        //Fichier de texte énorme
        const string PATH_INITIAL_FILE = @"C:\Users\admin\Desktop\LesMots\lorem5.txt";
        const string PATH = @"C:\Users\admin\Desktop\LesMots\";


        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            WordManager wm = new WordManager();
            DateTime start = DateTime.Now;
            sw.Start();
            wm.SplitFile(PATH_INITIAL_FILE);

            List<ConcurrentDictionary<string, int>> filesData = new List<ConcurrentDictionary<string, int>>();
            for (int i = 0; i < wm.NumberOfFiles; i++)
            {
                filesData.Add(wm.SplitWords(i));
                Debug.Print(i.ToString());
            }
            //!wordsTmp.ContainsKey(x.Key)) ? wordsTmp.Add(x.Key, x.Value) : wordsTmp[x.Key] += x.Value)
            Dictionary<string, int> wordsTmp = new Dictionary<string, int>();

            //Convert the list of dictonnaries into one
            foreach (var item in filesData)
            {
                if (item.Count > 0)
                {
                    foreach (var i in item)
                    {
                        if (!wordsTmp.ContainsKey(i.Key))
                        {
                            wordsTmp.Add(i.Key, i.Value);
                        }
                    }
                }
            }

            var listKeys = wordsTmp.Keys.ToList();
            wm.WriteOrderedFile(listKeys, wordsTmp);
            sw.Stop();
            DateTime end = DateTime.Now;

            using (System.IO.StreamWriter file =
             new System.IO.StreamWriter(PATH + "elapsedTime.txt"))
            {
                {
                    file.WriteLine("Temps écoulé : " + sw.Elapsed);
                    file.WriteLine("Heure de départ : " + start);
                    file.WriteLine("Heure de fin : " + end);
                }
            }
        }
    }
}
