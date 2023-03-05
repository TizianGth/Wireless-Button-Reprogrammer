using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Win32;

namespace HyperXCloud2
{
    public class Main
    {
        public bool Started { get; private set; } = false;

        public Device Device { get; private set; }

        public Main()
        {
            SetStartup();
        }

    private void SetStartup()
    {
        RegistryKey rk = Registry.CurrentUser.OpenSubKey
            ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (!rk.GetValueNames().Contains("HyperX"))
            {
                rk.SetValue("HyperX", Environment.CurrentDirectory + "//HyperXCloud2.exe");
            }


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
