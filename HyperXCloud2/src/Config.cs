using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperXCloud2
{
    public class Config
    {
        public string[] variables = new string[7];

        public Config()
        {

        }

        public Config(string _VendorID, string _ProductID, string _Interval, string _Keycode1, string _Keycode2, string _Keycode3, string _VolumeStep)
        {
            variables[0] = _VendorID;              // VendorID 
            variables[1] = _ProductID;            // ProductID
            variables[2] = _Interval;              // Interval 
            variables[3] = _Keycode1;              // Keycode1 
            variables[4] = _Keycode2;              // Keycode2 
            variables[5] = _Keycode3;              // Keycode3 
            variables[6] = _VolumeStep;              // Keycode3 
        }

        public override string ToString()
        {
            return "Vendor ID: " + variables[0] + "\n" +
                    "Product ID: " + variables[1] + "\n" +
                    "Interval: " + variables[2] + "\n" +
                    "Keycode1: " + variables[3] + "\n" +
                    "Keycode2: " + variables[4] + "\n" +
                    "Keycode3: " + variables[5] + "\n" +
                    "VolumeStep: " + variables[6];
        }
    }
}
