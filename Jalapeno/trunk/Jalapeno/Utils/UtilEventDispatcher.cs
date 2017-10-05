using System;
using System.Collections.Generic;

namespace Jalapeno.Utils
{
    //Event class that can hold event data and the sender object. It has an Invoke method which raises events
    public class Event
    {
        private event EventHandler fTarget; // Event
        private object fSender; 
        private EventArgs fArgs;

        public Event(EventHandler target, object sender, EventArgs args)
        {
            fTarget = target;
            fSender = sender;
            fArgs = args;
        }

        public void Invoke()
        {
            if (fTarget != null) //checking to see that there are fTarget subscribers
            {
                fTarget(fSender, fArgs); // Raising fTarget event
            }
        }
    }

    //Queue Class that stores events
    public class UtilEventDispatcherBase : EventDispatcher
    {
        private string fName;
        private Queue<Event> fQueue;

        //creator
        public UtilEventDispatcherBase(string name)
        {
            fName = name;
            fQueue = new Queue<Event>();
        }


        public string Name
        {
            get { return fName; }
        }


        //Enqueues events
        public virtual void Post(EventHandler target, object sender, EventArgs args)
        {
            Event e = new Event(target, sender, args);

            lock (fQueue)
            {
                fQueue.Enqueue(e);
            }
        }

        //Dequeues and raises events, will return TRUE if there if a valid event was dequeued and raised
        protected bool ProcessOne()
        {

            bool result = false;

            Event e = null;

            lock (fQueue)
            {
                if (fQueue.Count > 0)
                {
                    e = fQueue.Dequeue(); //Dequeue event
                }
            }

            if (e != null)
            {
                e.Invoke(); // Raise event
                result = true;
            }

            return result;
        }
    }
}
