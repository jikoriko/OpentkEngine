using OpenTK.Audio.OpenAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenTkEngine.Core
{

    static class SoundManager
    {
        static bool StackisRunning;
        static List<int> SourceStack = new List<int>();

        /// <summary>
        /// Constantly checks if a sound has stopped, if so removes it
        /// </summary>
        static void stackLoop()
        {
            StackisRunning = true;
            while (SourceStack.Count > 0)
            {
                foreach (int source in SourceStack)
                {

                    if (AL.GetSourceState(source) == ALSourceState.Stopped)
                    {
                        SourceStack.Remove(source);
                    }
                }
            }
            StackisRunning = false; // signals that the loop has stopped
        }
        public static void PlayInstance(int inBuffer)
        {
            int source = AL.GenSource();
            AL.Source(source, ALSourcei.Buffer, inBuffer);
            AL.SourcePlay(source);
            SourceStack.Add(source);
            if (!StackisRunning) stackLoop(); //restarts the loop
        }
    }


}
