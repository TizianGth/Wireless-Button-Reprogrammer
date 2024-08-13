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
        public static string EnvironmentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "//";
        public static void WriteToFile(string path, string text)
        {
            File.WriteAllText(EnvironmentPath + path, text);
        }
        public static string[] ReadLinesFromFile(string path)
        {
            if (!File.Exists(EnvironmentPath + path)) return null;

            return File.ReadLines(EnvironmentPath + path).ToArray();
        }
    }
}
