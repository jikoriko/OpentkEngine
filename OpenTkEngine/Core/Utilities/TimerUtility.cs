using System;

namespace OpenTkEngine.Core
{
    public class TimerUtility
    {
        DateTime mLastTime;

        public TimerUtility()
        {}

        public void Start()
        {
            mLastTime = DateTime.Now;
        }

        public float GetElapsedSeconds()
        {
            DateTime now = DateTime.Now;
            TimeSpan elasped = now - mLastTime;
            mLastTime = now;
            return (float)elasped.Ticks / TimeSpan.TicksPerSecond;
        }
    }
}
