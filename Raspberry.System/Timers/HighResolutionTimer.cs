#region References

using System;
using System.Threading;

#endregion

namespace Raspberry.Timers
{
    public class HighResolutionTimer : ITimer
    {
        #region Fields

        private decimal delay;
        private decimal interval;
        private Action action;

        private Thread thread;

        #endregion

        #region Instance Management

        public HighResolutionTimer()
        {
            if (!Board.Current.IsRaspberryPi)
                throw new NotSupportedException("Cannot use HighResolutionTimer on a platform different than Raspberry Pi");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the interval, in milliseconds.
        /// </summary>
        /// <value>
        /// The interval, in milliseconds.
        /// </value>
        public decimal Interval
        {
            get { return interval; }
            set
            {
                if (value > uint.MaxValue/1000)
                    throw new ArgumentOutOfRangeException("value", interval, "Interval must be lower than or equal to uint.MaxValue / 1000");

                interval = value;
            }
        }

        /// <summary>
        /// Gets or sets the action.
        /// </summary>
        /// <value>
        /// The action.
        /// </value>
        public Action Action
        {
            get { return action; }
            set
            {
                if (value == null)
                    Stop();

                action = value;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts this instance.
        /// </summary>
        /// <param name="startDelay">The delay before the first occurence, in milliseconds.</param>
        public void Start(decimal startDelay)
        {
            if (startDelay > uint.MaxValue/1000)
                throw new ArgumentOutOfRangeException("startDelay", startDelay, "Delay must be lower than or equal to uint.MaxValue / 1000");

            lock (this)
            {
                if (thread == null)
                {
                    delay = startDelay;
                    thread = new Thread(ThreadProcess);
                    thread.Start();
                }
            }
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            lock (this)
            {
                if (thread != null)
                {
                    if (thread != Thread.CurrentThread)
                        thread.Abort();
                    thread = null;
                }
            }
        }

        #endregion

        #region Private Helpers

        private void ThreadProcess()
        {
            var thisThread = thread;

            Sleep(delay);
            while (thread == thisThread)
            {
                (Action ?? NoOp)();
                Sleep(interval);
            }
        }

        private void NoOp(){}

        public static void Sleep(decimal delay)
        {
            Interop.bcm2835_delayMicroseconds((uint) (delay*1000));
        }

        #endregion
    }
}