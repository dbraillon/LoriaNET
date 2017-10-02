using System;
using System.Linq;

namespace LoriaNET
{
    /// <summary>
    /// A class that contains a set of listeners, allowing user to retrieve a specific 
    /// listener or to start/stop every listeners in set.
    /// </summary>
    public sealed class Listeners
    {
        /// <summary>
        /// A set of actions.
        /// </summary>
        private IListener[] Set { get; }

        /// <summary>
        /// Create an instance with a pre defined set of listeners.
        /// </summary>
        /// <param name="listeners">A set of listeners.</param>
        public Listeners(params IListener[] listeners)
        {
            Set = listeners;
        }

        /// <summary>
        /// Retrieve a listener by its name.
        /// </summary>
        /// <param name="name">A name of a listener.</param>
        /// <returns>Found listener or null.</returns>
        public IListener Get(string name) => Set.FirstOrDefault(l => string.Equals(l.Name, name, StringComparison.InvariantCultureIgnoreCase));
        
        /// <summary>
        /// Start every listener in set.
        /// </summary>
        public void StartAll()
        {
            foreach (var listener in Set)
            {
                listener.Start();
            }
        }

        /// <summary>
        /// Stop every listener in set.
        /// </summary>
        public void StopAll()
        {
            foreach (var listener in Set)
            {
                listener.Stop();
            }
        }

        /// <summary>
        /// Pause every listener in set.
        /// </summary>
        public void PauseAll()
        {
            foreach (var listener in Set)
            {
                listener.Pause();
            }
        }

        /// <summary>
        /// Resume every listener in set.
        /// </summary>
        public void ResumeAll()
        {
            foreach (var listener in Set)
            {
                listener.Resume();
            }
        }
    }
}
