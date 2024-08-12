using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBR
{
    /// <summary>
    /// Saves/reads all needed config variables and converts them to one single string
    /// Not a very good way, but good enough for now
    /// </summary>
    public class Config
    {
        public static string FileName = "config.yml";

        public string[] variables = new string[8];
        public Config() {}

        public Config(string _VendorID, string _ProductID, string _Interval, string _Keycode1, string _Keycode2, string _Keycode3, string _Device, bool _ShouldHideInTray)
        {
            variables[0] = _VendorID;             // VendorID 
            variables[1] = _ProductID;            // ProductID
            variables[2] = _Interval;             // Interval 
            variables[3] = _Keycode1;             // Keycode1 
            variables[4] = _Keycode2;             // Keycode2 
            variables[5] = _Keycode3;             // Keycode3 
            variables[6] = _Device;           // 
            variables[7] = _ShouldHideInTray.ToString();           // 
        }

        /// <summary>
        /// Saves the current config as a file
        /// </summary>
        public void SaveConfig()
        {
            FileHandler.WriteToFile(FileName, this.ToString());
        }

        /// <summary>
        /// Reads text from config.yml File and imports it as variables to the config class
        /// </summary>
        /// <returns></returns>
        public static Config LoadConfig()
        {
            var text = FileHandler.ReadLinesFromFile(FileName);
            if (text == null)
                return null;

            Config config = new Config();

            for (int i = 0; i < text.Length; i++)
            {
                config.variables[i] = text[i].Substring(text[i].IndexOf(':') + 2);
            }
            return config;
        }

        public override string ToString()
        {
            return
                "Vendor ID: "   + variables[0] + "\n" +
                "Product ID: "  + variables[1] + "\n" +
                "Interval: "    + variables[2] + "\n" +
                "Keycode1: "    + variables[3] + "\n" +
                "Keycode2: "    + variables[4] + "\n" +
                "Keycode3: "    + variables[5] + "\n" +
                "Device: "      + variables[6] + "\n" +
                "InTray: "      + variables[7];
        }


    }
}
