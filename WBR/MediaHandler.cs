using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace WBR
{
    /// <summary>
    /// Handles the user defined key codes and custom volume up/down step
    /// </summary>
    public static class MediaHandler
    {
        private const uint INPUT_KEYBOARD = 1;

        private const uint KEYEVENTF_KEYUP = 0x0002;
        private const uint KEYEVENTF_EXTENDEDKEY = 0x0001;


        // can be swapped by user to have a different functionality!
        public static byte NEXT = 0xB0;// keycode to jump to next track
        public static byte PLAY_PAUSE = 0xB3;// keycode to play or pause a song
        public static byte PREV = 0xB1;// keycode to jump to prev track

        public static void first()
        {
            SendMediaKey(PLAY_PAUSE);
        }
        public static void second()
        {
            SendMediaKey(NEXT);
        }
        public static void third()
        {
            SendMediaKey(PREV);
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct INPUT
        {
            public uint type;
            public InputUnion U;
        }

        [StructLayout(LayoutKind.Explicit, Size = 32)]
        private struct InputUnion
        {
            [FieldOffset(0)]
            public KEYBDINPUT ki;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(
            uint nInputs,
            INPUT[] pInputs,
            int cbSize);

        private static bool SendMediaKey(ushort key)
        {
            var inputs = new[]
            {
                new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = key,
                            wScan = 0,
                            dwFlags = KEYEVENTF_EXTENDEDKEY,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                },
                new INPUT
                {
                    type = INPUT_KEYBOARD,
                    U = new InputUnion
                    {
                        ki = new KEYBDINPUT
                        {
                            wVk = key,
                            wScan = 0,
                            dwFlags = KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
                            time = 0,
                            dwExtraInfo = IntPtr.Zero
                        }
                    }
                }
            };

            uint result = SendInput(
                (uint)inputs.Length,
                inputs,
                Marshal.SizeOf<INPUT>());

            return result == inputs.Length;
        }
    }
}
