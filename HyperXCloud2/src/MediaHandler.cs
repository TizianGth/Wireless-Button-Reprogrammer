using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HyperXCloud2
{
    public static class MediaHandler
    {
        // THESE CAN BE SWAPED TO DIFFERENT KEYCODES BY USER TO HAVE A DIFFERENT FUNCTION!!!
        public static byte NEXT = 0xB0;// keycode to jump to next track
        public static byte PLAY_PAUSE = 0xB3;// keycode to play or pause a song
        public static byte PREV = 0xB1;// keycode to jump to prev track

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte virtualKey, byte scanCode, uint flags, IntPtr extraInfo);
        public static void PauseResumeAudio()
        {
            keybd_event(PLAY_PAUSE, 0, 1, IntPtr.Zero);
        }
        public static void NextAudio()
        {
            keybd_event(NEXT, 0, 1, IntPtr.Zero);
        }
        public static void PrevAudio()
        {
            keybd_event(PREV, 0, 1, IntPtr.Zero);
        }
    }
}
