using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBR
{

    /// <summary>
    /// Handles basic file reading/writing
    /// </summary>
    internal class FileHandler
    {
        public static void WriteToFile(string path, string text)
        {
            File.WriteAllText(Environment.CurrentDirectory + "//" + path, text);
        }
        public static string[] ReadLinesFromFile(string path)
        {
            if (!File.Exists(Environment.CurrentDirectory + "//" + path)) return null;

            return File.ReadLines(Environment.CurrentDirectory + "//" + path).ToArray();
        }
    }
}
