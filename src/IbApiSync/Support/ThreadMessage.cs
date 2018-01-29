using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IbApiSync.Support
{
    /// <summary>
    /// This class contains just one property where any thread can store caught exception.
    /// This is the most simple way of handling exception from different thread I came up with.
    /// It works like this: A thread catches an exception. It saves the exception here and then interruptes main thread.
    /// Main thread reacts to ThreadInterruptedException and if caught, it looks for thread's exception into this property.
    /// </summary>
    public static class ThreadMessage
    {
        /// <summary>
        /// Place for interchanging caught exceptions among threads. It apparently isn't bullet proof, but enough for current usage. 
        /// </summary>
        public static Exception ThrownException { get; set; }
    }
}
