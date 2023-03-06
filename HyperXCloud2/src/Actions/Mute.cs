using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using HidLibrary;

namespace HyperXCloud2
{
    public class Mute : Device
    {
        private static readonly string[] MUTE_BYTES = new string[] { ByteToLookFor.MUTE_1, ByteToLookFor.MUTE_2, ByteToLookFor.MUTE_3, ByteToLookFor.MUTE_4 };
        public Mute(int vid, int pid) : base(vid, pid, 2, MUTE_BYTES) {}

        public override void Action()
        {
            ClickHandler.HandleClick();
        }
    }
}
