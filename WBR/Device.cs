using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HidLibrary;

namespace WBR
{
    /// <summary>
    /// Handles input from a HID device based on Vendor and Product ID
    /// </summary>
    public class Device
    {
        public Device(string device, int vid, int pid)
        {
            Vid = vid;
            Pid = pid;
            DeviceName = device;
        }

        private int Vid; // Vendor ID
        private int Pid; // Product ID
        private string DeviceName;
        List<Thread> threads;

        /// <summary>
        /// Goes through every single thread created previously and aborts them
        /// </summary>
        public void Stop()
        {
            if (threads == null) return;

            for(int i = 0; i < threads.Count; i++)
            {
                if (threads[i] != null) threads[i].Abort();
                threads.RemoveAt(i);
            }
        }

        /// <summary>
        /// Initializes HID device and opens a thread for each "sub-hid"
        /// </summary>
        /// <returns></returns>
        public int Init()
        {
            var f = HidDevices.Enumerate();
            var devices = HidDevices.Enumerate(Vid, Pid).ToList();

            // Initializing
            
            for (int i = 0; i < devices.Count(); i++)
            {
                HidDevice device = devices[i];

                if (device == null) 
                    break;

                device.OpenDevice();
                device.MonitorDeviceEvents = true;
            }
            
            MediaHandler.VOLUME_CURRENT = (int)VideoPlayerController.AudioManager.GetMasterVolume();

            // Creating threads
            threads = new List<Thread>(new Thread[devices.Count]);
            for (int i = 0; i < devices.Count(); i++)
            {
                HidDevice device = devices[i];
                threads[i] = new Thread(() =>
                {
                    while (device != null)
                    {
                        ReportHandler(device);
                    }
                });
                threads[i].Start();
            }
            return devices.Count;
        }

        private void ReportHandler(HidDevice device)
        {
            List<byte> data = device.ReadReport().Data.ToList();

            if (data.Count < 1)
                return;

            Console.WriteLine(BytesToString(data));

            if (DevicePresets.Contains("HyperX Cloud II Wireless (DTS)", data))
            {
                ClickHandler.HandleClick();
            }

        }

        private string BytesToString(List<byte> bytes)
        {
            string res = "{ ";
            int size = bytes.Count;
            for (int i = 0; i < size; i++)
            {
                res += "[" + (int)bytes[i] + "]";

                if (i != size - 1)
                {
                    res += ", ";
                }
                else
                {
                    res += " ";
                }

            }
            return res + "}";
        }

    }
}
