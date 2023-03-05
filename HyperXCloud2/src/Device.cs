using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HidLibrary;

namespace HyperXCloud2
{
    public class Device
    {
        public Device(int _vid, int _pid)
        {
            Vid = _vid;
            Pid = _pid;
        }

        // TODO: LET USER INPUT THEIR OWN
        public int Vid;// Vendor ID
        public int Pid; // Product ID

        // TODO: Maybe define this in a list of strings and just put the recieved bytes together to one string and compare
        private static List<byte[]> BYTES_TO_LOOK_FOR = new List<byte[]> // All the mesured bytes associated with pressing the "mute" button
        { new byte[] { 255, 187, 32, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 127, 0, 0 }, new byte[] { 255, 187, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 127, 0, 0 },
          new byte[] { 255, 187, 32, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, new byte[] { 255, 187, 32, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };


        private static HidDevice device;

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

                device = devices[2]; // HyperX splits its different controlls into different sub-HIDs; 2nd for mute button

                if (device == null) return;

                device.OpenDevice();

                device.MonitorDeviceEvents = true;

                while (device != null && device.IsConnected)
                {
                    ReportHandler();
                }

            });

            thread.Start();
        }

        private void ReportHandler()
        {
            byte[] data = device.ReadReport().Data;

            CheckForDesiredBytes(data);

            string output = "";
            for (int i = 0; i < data.Length; i++)
            {
                output += data[i] + " ";
            }

            Console.WriteLine(output);
        }

        private void CheckForDesiredBytes(byte[] data)
        {
            for (int sequence = 0; sequence < BYTES_TO_LOOK_FOR.Count(); sequence++)
            {
                if (data.Length == BYTES_TO_LOOK_FOR[sequence].Length)
                {
                    for (int i = 0; i < data.Length; i++)
                    {
                        if (data[i] != BYTES_TO_LOOK_FOR[sequence][i])
                        {
                            break;
                        }
                        if (i == data.Length - 1)
                        {
                            ClickHandler.HandleClick();
                            return;
                        }
                    }
                }
            }
        }
    }
}
