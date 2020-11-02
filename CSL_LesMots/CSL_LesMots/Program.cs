using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSL_LesMots
{
    class Program
    {
        //const string PATH = @"C:\Users\Administrateur\Desktop\LesMots\lorem5_mini.txt";
        const string PATH = @"C:\Users\Administrateur\Desktop\LesMots\lorem5.txt";
        static void Main(string[] args)
        {
            WordManager wm = new WordManager();
            wm.LoadWords(PATH);
            wm.WriteOrderedFile();
            Console.ReadLine();
        }
    }
}
