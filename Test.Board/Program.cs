using System;
using System.Diagnostics;
using Raspberry;

namespace Test.Board
{
    class Program
    {
        static void Main()
        {
            // Setup tracing for test app
            Tracing.raspberrySystemTraceSwitch.Level = TraceLevel.Verbose;
            Trace.Listeners.Add( new ConsoleTraceListener() );

            var board = Raspberry.Board.Current;


            if ( !board.IsRaspberryPi )
                Console.WriteLine( "System is not a Raspberry Pi" );
            else
            {
                Console.WriteLine( "Raspberry Pi running on {0} processor.", board.Processor );
                Console.WriteLine( board );
            }
        }
    }
}
