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

        public Device Device { get; private set; }

        public Main()
        {
            //Start(0x03F0, 0x0696);
        }

        public void Start(int Vid, int Pid)
        {
            if (Started) return;

            Device = new Device(Vid, Pid);
            Device.Init();

            Started = true;
        }
        public void Stop()
        {
            if(!Started) return;
            Device.Stop();
            Device = null;
            Started = false;
        }
    }
}
