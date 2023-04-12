using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HidLibrary;

namespace HyperXCloud2
{
    public class Device
    {
        public Device(int vid, int pid)
        {
            Vid = vid;
            Pid = pid;
        }

        public int Vid; // Vendor ID
        public int Pid; // Product ID

        Thread thread;

        public void Stop()
        {
            thread.Abort();
        }

        public int Init()
        {
            var devices = HidDevices.Enumerate(Vid, Pid).ToList();

            for (int i = 0; i < devices.Count(); i++)
            {
                if (devices.Count == 0)
                {
                    return 0;
                }

                HidDevice device = devices[i];

                if (device == null) break;

                device.OpenDevice();

                device.MonitorDeviceEvents = true;
            }

            Debug.Write("test");

            for (int i = 0; i < devices.Count(); i++)
            {
                HidDevice device = devices[i];

                // New thread to not freeze gui
                thread = new Thread(() =>
                {

                    while (device != null)
                    {
                        ReportHandler(device);
                    }
                    Thread.Sleep(5); // 1000 / 5 => RportHandler Gets called 200 times a second

                });
                thread.Start();
            }
            

            return devices.Count;

        }

        private bool CheckForValidDevices(ref List<HidDevice> devices)
        {
            return devices.Count > 0;
        }

        private void ReportHandler(HidDevice device)
        {
            byte[] data = device.ReadReport(5).Data;

            string byteStr = BytesToString(data);

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

            // REMOVE UNUSED "DEVICE" FROM DEVICES TO SAVE PROCESSING POWER
            else
            {

            }

        }

        private bool ContainsByte(string bytes, string[] bytesWanted)
        {
            for (int sequence = 0; sequence < bytesWanted.Length; sequence++)
            {
                if (bytes == bytesWanted[sequence])
                {
                    return true;
                }
            }
            return false;
        }
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
