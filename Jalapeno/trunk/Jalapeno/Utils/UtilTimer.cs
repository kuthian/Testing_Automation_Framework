using System;


namespace Jalapeno.Utils
{
    public class UtilTimer
    {
        private EventDispatcher fDispatcher;
        private System.Timers.Timer fSystemTimer;

        public event EventHandler TimerElapsed;

        public UtilTimer(EventDispatcher dispatcher)
        {
            fDispatcher = dispatcher;
            fSystemTimer = new System.Timers.Timer();
            fSystemTimer.Elapsed += fSystemTimer_OnTimerElapsed;
            fSystemTimer.AutoReset = false;
        }

        //method subscribed to fSystemTimer.Elapsed (i.e. When the timer is finished counting)
        void fSystemTimer_OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (TimerElapsed != null) // checking to see that there are OnTimer subscribers
            {
                if (fDispatcher != null) // note that fdispatcher appears to always be null
                {
                    //Console.WriteLine("Queueing OnTimer event");
                    fDispatcher.Post(TimerElapsed, this, EventArgs.Empty); // queues event at the dispatcher base
                }
                else
                {
                    //Console.WriteLine("OnTimer event raised directly");
                    TimerElapsed(this, EventArgs.Empty); // raise OnTimer Event
                }
            }
        }

        public long Interval
        {
            get
            {
                return Convert.ToInt64(fSystemTimer.Interval);
            }
            set
            {
                fSystemTimer.Interval = value;
            }
        }

        public void Start(long milliseconds)
        {
            Interval = milliseconds;
            Start();
        }

        public void Start()
        {
            fSystemTimer.Start();
        }

        public void Stop()
        {
            fSystemTimer.Stop();
        }

        public bool IsRunning
        {
            get { return fSystemTimer.Enabled; }
        }
    }
}
