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

        public Device testDevice { get; private set; }
        public Mute muteDevice { get; private set; }
        public VolumeUp volumeUpDevice { get; private set; }
        public VolumeDown volumeDownDevice { get; private set; }

        public Main()
        {
        }


    public void Start(int Vid, int Pid)
        {
            if (Started) return;

            MediaHandler.VOLUME_CURRENT = (int)VideoPlayerController.AudioManager.GetMasterVolume();

            //testDevice = new Device(Vid, Pid, 0, null);
            //testDevice.Init();

            muteDevice = new Mute(Vid, Pid);
            muteDevice.Init();

            volumeUpDevice = new VolumeUp(Vid, Pid);
            volumeUpDevice.Init();

           volumeDownDevice = new VolumeDown(Vid, Pid);
           volumeDownDevice.Init();

            Started = true;
        }
        public void Stop()
        {
            if(!Started) return;

            //testDevice.Stop();
           //testDevice = null;

            muteDevice.Stop();
            muteDevice = null;

            volumeUpDevice.Stop();
            volumeUpDevice = null;

            volumeDownDevice.Stop();
            volumeDownDevice = null;

            Started = false;
        }
    }
}
