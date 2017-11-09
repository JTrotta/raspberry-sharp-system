
using System.Diagnostics;

namespace Raspberry
{
    /// <summary>
    /// Adds tracing support for the assembly.
    /// </summary>
    public class Tracing
    {
        /// <summary>
        /// Trace switch for the Raspberry.System assembly.
        /// </summary>
        public static TraceSwitch raspberrySystemTraceSwitch = new TraceSwitch("RPi-System", "Raspberry.System trace switch", "Off");
        internal const string traceCat = "RPi-System";

        public static void TraceError( string message )
        {
            Trace.WriteLineIf(raspberrySystemTraceSwitch.TraceError, message, traceCat);
        }
    }
}
