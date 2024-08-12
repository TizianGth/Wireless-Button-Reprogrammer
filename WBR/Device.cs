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
        public Device(int vid, int pid)
        {
            Vid = vid;
            Pid = pid;
        }

        public int Vid; // Vendor ID
        public int Pid; // Product ID
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
                //.ReadReport(OnReport);
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
            return devices.Count; // TODO: don't return device count from Init function
        }

        /// <summary>
        /// Reads data from device, converts it to string and then compares
        /// it to the bytes messured 
        /// </summary>
        /// <param name="device"></param>
        /// 

        private void OnReport(HidReport report)
        {
            byte[] data = report.Data;

            if (data.Length < 1)
                return;

            //Console.WriteLine(data);
            string byteStr = BytesToString(data);

            Console.WriteLine(ByteArrayToString(data));



            if (ContainsByte(byteStr, Bytes.VOLUME_UP))
            {
                MediaHandler.VolumeUp();
            }
            else if (ContainsByte(byteStr, Bytes.VOLUME_DOWN))
            {
                MediaHandler.VolumeDown();
            }
            else if (ContainsByte(byteStr, Bytes.MUTE))
            {
                ClickHandler.HandleClick();
            }

        }

        private void ReportHandler(HidDevice device)
        {
            byte[] data = device.ReadReport().Data;

            if (data.Length < 1)
                return;

            string byteStr = BytesToString(data);

            Console.WriteLine(ByteArrayToString(data));
            /*
            if (ContainsByte(byteStr, Bytes.VOLUME_UP))
            {
                MediaHandler.VolumeUp();
            }

            else if (ContainsByte(byteStr, Bytes.VOLUME_DOWN))
            {
                MediaHandler.VolumeDown();
            }*/
            if (ContainsByte(byteStr, Bytes.MUTE))
            {
                ClickHandler.HandleClick();
            }
          
        }

        private string ByteArrayToString(byte[] arr)
        {
            string res = "{ ";
            int size = arr.Count();
            for (int i = 0; i < size; i++)
            {
                res += "[" + (int)arr[i] + "]";

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


        /// <summary>
        /// loops through each predefined string to check if a new string matches
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="bytesWanted"></param>
        /// <returns></returns>
        private bool ContainsByte(string bytes, string[] bytesWanted)
        {
            for (int i = 0; i < bytesWanted.Length; i++)
            {
                if (bytes == bytesWanted[i])
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Converts byte array to string
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string BytesToString(byte[] data)
        {
            string result = "";
            for (int i = 0; i < data.Length; i++)
            {
                result += data[i];
            }
            return result;
        }

    }
}
