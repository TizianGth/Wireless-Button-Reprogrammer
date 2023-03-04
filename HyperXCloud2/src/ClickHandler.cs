using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HyperXCloud2
{
    public static class ClickHandler
    {
        public static int ClickInterval = 500; // Time between clicks in which we detect the users input

        private static Stopwatch clock; // tracks time between clicks
        private static int censecutiveClicks = 0; // tracks number of consecutive clicks in one interval

        public static void HandleClick()
        {
            // Starts Clock at the very first click or if interval elapsed
            if (clock == null || clock.ElapsedMilliseconds > ClickInterval)
            {
                censecutiveClicks = 0;

                clock = Stopwatch.StartNew();
            }
            censecutiveClicks++;
         
            ClickAction(censecutiveClicks);
        }
        private static void ClickAction(int clicks)
        {
            if(clicks == 1)
            {
                MediaHandler.PauseResumeAudio();
            } else if(clicks == 2)
            {
                // We cant determin if user will press button 2 or 3 times -> we wait until either 1. user pressed 3 times (max) or 2. user pressed
                // 2 times and interval elapsed
                WaitForIntervalToFinish();
            }
            else if (clicks == 3)
            {
                MediaHandler.PrevAudio();
                censecutiveClicks = 0;
            }
        }

        private static void WaitForIntervalToFinish()
        {
            // Start a new seperate thread
            new Thread(() =>
            {
                // "Wait" until interval elapsed
                while (clock.ElapsedMilliseconds < ClickInterval)
                {

                }
                // Check if in the elapsed time frame another click occurred
                if (censecutiveClicks == 2)
                {
                    MediaHandler.NextAudio();
                    censecutiveClicks = 0;
                }
                return;
            }).Start();
        }
    }
}

