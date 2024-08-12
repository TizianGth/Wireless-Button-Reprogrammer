using System;
using System.Linq;

using HidLibrary;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;

namespace testusb
{
    internal class Program
    {
        static HashSet<List<Byte>> UselessBytes = new HashSet<List<byte>>();
        public class USB
        {
            public USB(int vid, int pid, int id)
            {
                Vid = vid;
                Pid = pid;
                Id = id;
            }

            public USB(HidDevice device)
            {
                string _v = device.DevicePath.Split("vid_")[1];
                string _p = _v.Split("pid_")[1];
                string v = _v.Substring(0, 4);
                string p = _p.Substring(0, 4);
                string _id = _p.Contains("col") ? _p.Split("col")[1].Substring(0, 2) : "0";
                HexVid = v;
                HexPid = p;
                int vid = Convert.ToInt32(v, 16);
                int pid = Convert.ToInt32(p, 16);
                int id = Convert.ToInt32(_id);

                Vid = vid;
                Pid = pid;
                Id = id;
            }

            public override string ToString()
            {
                return "Id: "+ Id + ", " + "Vid: " + HexVid + ", Pid: " + HexPid + " (Hexadecimal)";
            }


            public override bool Equals(object obj)
            {
                if (obj is USB)
                {
                    USB other = (USB)obj;
                    return Vid == other.Vid && Pid == other.Pid && Id == other.Id;
                }
                return false;
            }

            public override int GetHashCode()
            {
                unchecked // Overflow is fine, just wrap
                {
                    int hash = 17;
                    hash = hash * 23 + Vid.GetHashCode();
                    hash = hash * 23 + Pid.GetHashCode();
                    hash = hash * 23 + Id.GetHashCode();
                    return hash;
                }
            }




            private string HexVid = "";
            private string HexPid = "";

            private int Vid = 0;
            private int Pid = 0;
            private int Id = 0;
        }

        static void MessageWait(string message)
        {
            Console.WriteLine(message);
            Console.WriteLine("   Press any key to continue\n");
            Console.ReadKey();

            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }
        static void OnReport(HidReport report)
        {
            string result = "";
            List<byte> bytes = report.Data.ToList();
            foreach (byte b in bytes)
            {
                result += ((int)b) + " ";
            }
            //if(!UselessBytes.Contains(bytes))
                Console.WriteLine("Bytes: " + result);
        }

        static void GatherUselessBytes(HidReport report)
        {
            UselessBytes.Add(report.Data.ToList());
           // Console.WriteLine("");

        }

        static async Task Main(string[] args)
        {
            MessageWait("1. Please make sure your usb reciever is plugged in.");
            var devicesBefore = HidDevices.Enumerate().ToList();

            MessageWait("2. Please remove the usb reviever and ensure that no other usb devices are disconnecting randomly");
            var devicesAfter = HidDevices.Enumerate().ToList();
            var devicesAfterHash = new Dictionary<USB, HidDevice>();
            foreach (var d in devicesAfter)
            {
                var key = new USB(d);
                if (!devicesAfterHash.ContainsKey(key))
                {
                    devicesAfterHash[key] = d;
                }
            }
            bool found = false;
            Dictionary<USB, HidDevice> uniqueDevices = new Dictionary<USB, HidDevice>();

            foreach (HidDevice device in devicesBefore)
            {
                var usb = new USB(device);
                if (!devicesAfterHash.ContainsKey(usb))
                {
                    if(!uniqueDevices.ContainsKey(usb))
                    {
                        uniqueDevices.Add(usb, device);
                        Console.WriteLine(usb);
                        found = true;
                    }
                }
            }


#if DEBUG
            {
                // test override
                int vid = Convert.ToInt32("03F0", 16);
                int pid = Convert.ToInt32("0696", 16);

                var devicesListTemp = HidDevices.Enumerate(vid, pid).ToList();
                foreach (var device in devicesListTemp)
                {
                    var usb = new USB(device);
                    uniqueDevices.Add(usb, device);
                }

                found = true;

                Console.WriteLine("DEBUG MODE ACTIVATE!\n");
            }
#endif


            if (!found)
            {
                Console.WriteLine("Couldn't find your device.");
                Console.ReadKey();
                return;
            }

            
            


            MessageWait("3. Ensure there are no sounds playing");

            
            List<Thread> threads = new List<Thread>();
            bool stop = false;
            
            foreach (var device in uniqueDevices.Values)
            {
                device.OpenDevice();
                Thread t = new Thread(async () =>
                {
                    while (!stop)
                    {
                        HidReport report = device.ReadReport();
                        if(!stop)
                        GatherUselessBytes(report);
                    }
                });
                t.Start();
                threads.Add(t);

            } 
            MessageWait("4. Wait ~30s-60s to be safe before continuing.");
            
            stop = true;
            foreach (var device in uniqueDevices.Values)
            {
                device.CloseDevice();
            }
            foreach (var t in threads)
            {
                t.Interrupt();
                t.Join();
            }

            MessageWait("5. After continuing click the desired button on your headset multiple times and you will see the required bytes \n   [The more data the better :)]");

            threads = new List<Thread>();
            stop = false;
            foreach (var device in uniqueDevices.Values)
            {
                device.OpenDevice();
                Thread t = new Thread(async () =>
                {
                    while (!stop)
                    {
                        HidReport report = device.ReadReport();
                        if (!stop)
                            OnReport(report);
                    }
                });
                t.Start();
                threads.Add(t);

            }

            while (true) ;


            /*

            Console.WriteLine("Found {0} devices", allDevices.Count);

            foreach (var device in allDevices)
            {
                Console.WriteLine("Got device: {0}\r\n", usbRegistry.FullName);

                if (usbRegistry.Open(out usbDevice))
                {
                    Console.WriteLine("Device Information\r\n------------------");

                    Console.WriteLine("{0}", usbDevice.Info.ToString());

                    Console.WriteLine("VID & PID: {0} {1}", usbDevice.Info.Descriptor.VendorID, usbDevice.Info.Descriptor.ProductID);

                    Console.WriteLine("\r\nDevice configuration\r\n--------------------");
                    foreach (UsbConfigInfo usbConfigInfo in usbDevice.Configs)
                    {
                        Console.WriteLine("{0}", usbConfigInfo.ToString());

                        Console.WriteLine("\r\nDevice interface list\r\n---------------------");
                        IReadOnlyCollection<UsbInterfaceInfo> interfaceList = usbConfigInfo.InterfaceInfoList;
                        foreach (UsbInterfaceInfo usbInterfaceInfo in interfaceList)
                        {
                            Console.WriteLine("{0}", usbInterfaceInfo.ToString());

                            Console.WriteLine("\r\nDevice endpoint list\r\n--------------------");
                            IReadOnlyCollection<UsbEndpointInfo> endpointList = usbInterfaceInfo.EndpointInfoList;
                            foreach (UsbEndpointInfo usbEndpointInfo in endpointList)
                            {
                                Console.WriteLine("{0}", usbEndpointInfo.ToString());
                            }
                        }
                    }
                    usbDevice.Close();
                }
                Console.WriteLine("\r\n----- Device information finished -----\r\n");
            }



            Console.WriteLine("Trying to find our device: {0} {1}", 0x03F0, 0x0696);
            UsbDeviceFinder usbDeviceFinder = new UsbDeviceFinder(0x03F0, 0x0696);

            // This does not work !!! WHY ?
            usbDevice = UsbDevice.OpenUsbDevice(usbDeviceFinder);

            if (usbDevice != null)
            {
                Console.WriteLine("OK");
            }
            else
            {
                Console.WriteLine("FAIL");
            }

            UsbDevice.Exit();

            Console.Write("Press anything to close");
            Console.ReadKey();
            */
        }

        private static void DeviceAttachedHandler()
        {
            //_attached = true;
            Console.WriteLine("Gamepad attached.");
            // _device.ReadReport(OnReport);
        }

        private static void DeviceRemovedHandler()
        {
            //_attached = false;
            Console.WriteLine("Gamepad removed.");
        }
    }

}