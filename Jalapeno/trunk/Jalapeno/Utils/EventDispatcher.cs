using System;

namespace Jalapeno.Utils
{
    public interface EventDispatcher
    {
        string Name { get; }
        void Post(EventHandler target, object sender, EventArgs args);
    }
}