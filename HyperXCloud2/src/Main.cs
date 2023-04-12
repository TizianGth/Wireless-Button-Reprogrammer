using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HyperXCloud2
{
    public class Main
    {
        public bool Started { get; private set; } = false;

        public Device device { get; private set; }

        public Main()
        {
        }

        int devices = 0;
        public int Start(int Vid, int Pid)
        {

            if (Started) return devices;

            MediaHandler.VOLUME_CURRENT = (int)VideoPlayerController.AudioManager.GetMasterVolume();

            device = new Device(Vid, Pid);
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
