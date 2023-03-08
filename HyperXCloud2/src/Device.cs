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
        public Device(int vid, int pid, int index, string[] bytes)
        {
            Vid = vid;
            Pid = pid;
            Index = index;
            Bytes = bytes;
        }

        public int Vid; // Vendor ID
        public int Pid; // Product ID
        public int Index; // Sub-HID device ID 2 = Mute; 0 = Volume Conmtroll
        public string[] Bytes;

        private HidDevice device;


        Thread thread;

        public void Stop()
        {
            thread.Abort();
        }

        public void Init()
        {
            
            // New thread to not freeze gui
            thread = new Thread(() =>
            {
                var devices = HidDevices.Enumerate(Vid, Pid).ToArray();

                if (devices.Length == 0) return;

                device = devices[Index]; 

                if (device == null) return;

                device.OpenDevice();

                device.MonitorDeviceEvents = true;

                while (device != null)
                {
                    ReportHandler();
                }

            });

            thread.Start();

        }

        private void ReportHandler()
        {
            if (Index == 0)
            {
            }
            byte[] data = device.ReadReport().Data;

            CheckForDesiredBytes(data);
        }

        private void CheckForDesiredBytes(byte[] data)
        {
            string byteStr = BytesToString(data);

            if (byteStr.Length == 0) return;
            if(Bytes == null || Bytes.Length == 0) return;

            if (ContainsByte(byteStr)) return;

        }

        private bool ContainsByte(string bytes)
        {
            for (int sequence = 0; sequence < Bytes.Length; sequence++)
            {
                if (bytes == Bytes[sequence])
                {
                    Action();
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

        public virtual void Action() { }
    }
}
