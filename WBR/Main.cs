using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBR
{
    /// <summary>
    /// Handles the start and stop process
    /// </summary>
    public class Main
    {
        public bool Started { get; private set; } = false;
        public Device device { get; private set; }
        int devices = 0;

        public Main(){}

        public int Start(string deviceName, int vid, int pid)
        {
            if (Started) return devices;

            device = new Device(deviceName, vid, pid);
            devices = device.Init();

            Started = true;

            return devices;
        }
        public void Stop()
        {
            if(!Started) return;

            device.Stop();
            device = null;

            Started = false;
        }

    }
}
