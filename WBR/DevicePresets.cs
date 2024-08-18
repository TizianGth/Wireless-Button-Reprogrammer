using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBR
{
    internal class DevicePresets
    {
        private static Dictionary<string, DevicePresets> presets = new Dictionary<string, DevicePresets>() {
            { "HyperX Cloud II Wireless (DTS)",
                new DevicePresets(new List<List<byte>>() { 
                    new List<byte>() { 255, 187, 32, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                    new List<byte>() { 255, 187, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
                    new List<byte>() { 255, 187, 32, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 127, 0, 0 }, 
                    new List<byte>() { 255, 187, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 127, 0, 0 }, 
                })
            },

            /* adding a new device
            { "PLACEHOLDER",
                new DevicePresets(new List<List<byte>>() {
                    new List<byte>() { 0, 0, 0, 0},
                    new List<byte>() { 0, 0, 0, 0 },
                    new List<byte>() { 0, 0, 0, 0 },
                    new List<byte>() { 0, 0, 0, 0 },
                })
            },
            */


        };

        public static bool Contains(string device, List<byte> bytes) 
        {
            if(presets.ContainsKey(device))
            {
                foreach(var byteList in presets[device].mute)
                {
                    if (bytes.Count == byteList.Count)
                    {
                        bool same = true;
                        for (int i = 0; i < bytes.Count; i++)
                        {
                            if (bytes[i] != byteList[i])
                                same = false;

                        }
                        if (same)
                            return true;
                    }

                }
            }
            return false;
        }



        
        private DevicePresets(List<List<byte>> bytes)
        {
            mute = bytes;
        }

        private List<List<byte>> mute;
    }


}
