using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WBR
{

    /// <summary>
    /// Handles basic file reading/writing
    /// </summary>
    internal class FileHandler
    {

        public static string EnvironmentPath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "//";
        static string appdata = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\WBR\\";
        public static void WriteToFile(string path, string text)
        {
            File.WriteAllText(EnvironmentPath + path, text);
        }
        // TODO: actually write to appdata!
        public static void WriteToAppData(string path, string text)
        {
            System.IO.Directory.CreateDirectory(appdata);
            File.WriteAllText(appdata + path, text);
        }
        public static string ReadFromAppData(string path)
        {
            if (!File.Exists(appdata + path)) return null;

            return File.ReadAllText(appdata + path);
        }
        public static string[] ReadLinesFromFile(string path)
        {
            if (!File.Exists(EnvironmentPath + path)) return null;

            return File.ReadLines(EnvironmentPath + path).ToArray();
        }
    }
}
