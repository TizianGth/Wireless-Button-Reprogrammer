using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBR
{
    public class Bytes
    {
        /// <summary>
        /// All the mesured bytes associated with pressing the "mute" button
        /// Using strings instead of byte array, because its easier to input strings
        /// Could cause problems when eg input = [255] with length 1 gets compared
        /// to string "255" cant tell if its[2],[5],[5] or just[255] => may cause false positive
        /// TODO: Dont use strings, when testing is done
        /// </summary>
        public static string[] MUTE = { "25518732100000000000012700", "25518732000000000000012700", "255187321000000000000000", "255187320000000000000000",
                                        "018780100000000000000000000000000000000000000000000000000000000", "018781100000000000000000000000000000000000000000000000000000000" };
        public static string[] VOLUME_UP = { "1000", "1" };
        public static string[] VOLUME_DOWN = { "2000", "2" };
    }
}
