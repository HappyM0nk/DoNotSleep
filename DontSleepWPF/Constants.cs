using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DontSleepWPF
{
    internal class Constants
    {
        public enum KeyPressorStatus { Runned = 1, Stopped = 2, Faulted = 3, StopTimerStarted = 4, StopTimerStopped = 5 }

        public enum TimePeriod { None = 0, Second = 1, Minute = 2, Hour = 3, Day = 4, Week = 5, Month = 6 }

    }

    
}
