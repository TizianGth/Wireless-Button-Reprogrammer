using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperXCloud2
{
    internal class FileHandler
    {
        public static void SaveConfig(Config config)
        {
            WriteToFile("config.yml", config.ToString());
        }
        public static Config LoadConfig()
        {
            Config config = new Config();
            string[] text = ReadLinesFromFile("config.yml");

            for(int i = 0; i < text.Length; i++)
            {
                config.variables[i] = text[i].Substring(text[i].IndexOf(": ") + 1);
            }
            return config;
        }
        public static void WriteToFile(string path, string text)
        {
            File.WriteAllText(Environment.CurrentDirectory + "//" + path, text);
        }
        public static string[] ReadLinesFromFile(string path)
        {
            return File.ReadLines(Environment.CurrentDirectory + "//" + path).ToArray();
        }
    }
}
