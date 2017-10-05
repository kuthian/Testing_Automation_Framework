using System;
using System.Threading;


namespace Jalapeno.Utils
{
    //Threading class which implements events to start/stop threads
    public class UtilThread : UtilEventDispatcherBase
    {
        private Thread fThread;
        private AutoResetEvent fSignal;
        private bool fRun;

        //Utilthread creator which also will create an EvenDispatcherBase
        public UtilThread(string name)
            : base(name)
        {
            fThread = null;
            fSignal = null;
            fRun = false;
        }

        //Start Thread
        public void Start()
        {
            fThread = new Thread(new ThreadStart(Run));
            fThread.Name = this.Name;
            fSignal = new AutoResetEvent(false);
            fRun = true;
            fThread.Start();
        }

        //Stop Thread safely
        public void Stop()
        {
            if (fRun)
            {
                fRun = false;
                fSignal.Set();

                fThread.Join();
                fThread = null;
            }
        }

        //Stop Thread instantly
        public void SelfExit()
        {
            if (fRun)
            {
                fRun = false;
                fSignal.Set();
                fThread = null;
            }
        }


        //Method that processes thread events until there are no more
        private void Run()
        {
            fRun = true;

            while (fRun)
            {
                while (this.ProcessOne())
                {
                    // Consume all events before going to sleep
                }

                fSignal.WaitOne(10000); // Wait for a signal for a maximum of 10 second; This is broken by fsignal.Set() method call from another thread
            }
        }

        //Send Event to base queue so that it will be raised when thread is Run()ing.
        public override void Post(EventHandler target, object sender, EventArgs args)
        {
            //Console.WriteLine("UtilThread: "); // debug
            base.Post(target, sender, args);
            //Console.WriteLine("Event posted ");
            fSignal.Set(); // If Run() is waiting (WaitOne), this will break it out
            //Console.WriteLine("Signal Set ");
        }
    }
}
