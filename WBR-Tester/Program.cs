using HidLibrary;
using System.Collections;

namespace WBR_Tester.src
{
    internal class Programm
    {
        static List<HidDevice> devices = null;
        static HashSet<byte[]> keySet = new HashSet<byte[]>(new ByteArrayComparer());
        static List<Thread> threads;
        static void Main(string[] args)
        {
           // string vid = "03F0";
           // string pid = "0696";

            
            Console.WriteLine("Enter your device's Vendor ID (VID):");

            // Create a string variable and get user input from the keyboard and store it in the variable
            string vid = Console.ReadLine();
            Console.WriteLine("");

            Console.WriteLine("Enter your device's Product ID (PID):");
            string pid = Console.ReadLine();
            Console.WriteLine("");

            string yn = "";
            while (!(yn.ToLower() == "y" || yn.ToLower() == "n"))
            {
                Console.WriteLine("VID: " + vid + " and PID: " + pid + " correct (y/n)");
                yn = Console.ReadLine();
                Console.WriteLine("");
            }

            if (vid == null || pid == null || vid == "" || pid == "")
            {
                Console.WriteLine("VID or PID empty. Please try again!");
                return;
            }

            try
            {
                devices = HidDevices.Enumerate(ParseHexStringToInt(vid), ParseHexStringToInt(pid)).ToList();
            }
            catch
            {
                Console.WriteLine("VID or PID invalid. Please try again!");
                return;
            }
            if (devices == null || devices.Count() < 1)
            {
                Console.WriteLine("VID or PID invalid. Please try again!");
                return;
            }

            threads = new List<Thread>(new Thread[devices.Count]);

            Console.WriteLine("Succsess!\n");
            Console.WriteLine("To obtain all the necessary data, please follow these instructions:");
            Console.WriteLine("1. Don't press any buttons on your headphone or do anything similar!");
            Console.WriteLine("2. Deactivate the device's speaker and microphone in windows or switch to them to another device and wait a bit.");

            yn = "";
            while (!(yn.ToLower() == "y" || yn.ToLower() == "n"))
            {
                Console.WriteLine("understood? (y/n)");
                yn = Console.ReadLine();
                Console.WriteLine("");
            }

            Console.WriteLine("3. Let this application open for some time. The exact time you need is something you'll have to figure out on your own, maybe start with 30 seconds, because this depends on how accurate the output is and this process also takes up a lot of memory. " +
                "\nAfter some time please press (enter)");

            for (int i = 0; i < devices.Count(); i++)
            {
                HidDevice device = devices[i];
                threads[i] = new Thread(() =>
                {
                    while (device != null)
                    {
                        AnalyseIncomingBytes(device);
                    }
                });
                threads[i].Start();

            }

            Console.ReadLine();

            analysed = true;

            Console.WriteLine("4. Press the desired button exactly 10 times but leave a small space of about 2-3 seconds between each click.");
            Console.ReadLine();


            Console.WriteLine("5. These are the detected bytes. Please copy them and contact me, because I still have to analyse them by hand.");
            PrintRelevantBytes();

            relevantBytes.Clear();
            keySet.Clear();

            foreach (Thread t in threads)
            {
                if (t != null) t.Interrupt();
            }
            threads.Clear();
        }


        static bool analysed = false;
        //static List<byte[]> relevantBytes = new List<byte[]>();
        static HashSet<byte[]> relevantBytes = new HashSet<byte[]>(new ByteArrayComparer());
        static void AnalyseIncomingBytes(HidDevice device)
        {
            byte[] data = device.ReadReport().Data;

            //Console.WriteLine(ByteArrayToString(data));

            if (!keySet.Contains(data))
            {
                if (!analysed)
                {
                    keySet.Add(data);
                }
                else
                {
                    if (!relevantBytes.Contains(data))
                        relevantBytes.Add(data);
                }
            }
        }

        static void PrintRelevantBytes()
        {
            foreach (byte[] b in relevantBytes)
            {
                Console.WriteLine("     " + ByteArrayToString(b));
            }
        }

        static string ByteArrayToString(byte[] arr)
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

        static private int ParseHexStringToInt(string text)
        {
            int result = int.Parse(text, System.Globalization.NumberStyles.HexNumber);
            return result;
        }

        class ByteArrayComparer : IEqualityComparer<byte[]>
        {
            public bool Equals(byte[] x, byte[] y)
            {
                return StructuralComparisons.StructuralEqualityComparer.Equals(x, y);
            }

            public int GetHashCode(byte[] obj)
            {
                return StructuralComparisons.StructuralEqualityComparer.GetHashCode(obj);
            }
        }

    }

}