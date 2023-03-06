using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HidLibrary;

namespace HyperXCloud2
{
    public class VolumeUp : Device
    {
        private static readonly string[] VOLUME_UP_BYTES = new string[] { ByteToLookFor.VOLUME_UP };
        public VolumeUp(int vid, int pid) : base(vid, pid, 0, VOLUME_UP_BYTES) { }

        public override void Action()
        {


            MediaHandler.VolumeUp();
        }
    }
}
