using System.Diagnostics;
using System.Threading;

namespace Loria.Core
{
    public class Engine
    {
        public bool IsLiving { get; private set; }

        public Modules Modules { get; set; }
        public Actions Actions { get; }
        public Callbacks Callbacks { get; }
        public Listeners Listeners { get; }
        public Data Data { get; set; }
        public IPropagator Propagator { get; set; }

        public Engine()
        {
            Debug.Listeners.Add(new ConsoleTraceListener());

            // Configure all modules
            Modules = new Modules(this);
            Modules.ConfigureAll();
            
            // Initialize actions, callbacks and listeners
            Actions = new Actions(Modules.GetAll<IAction>());
            Callbacks = new Callbacks(Modules.GetAll<ICallback>());
            Listeners = new Listeners(Modules.GetAll<IListener>());

            // Data
            Data = new Data(this);

            // Propagator
            Propagator = new Propagator(this);
        }

        public void Live()
        {
            // Fallback to LiveAsync
            LiveAsync();

            // Block current thread
            while (IsLiving) Thread.Sleep(1000);
        }
        public void LiveAsync()
        {
            // Turn the flag on
            IsLiving = true;

            // Start enabled listeners
            Listeners.StartAll();
        }

        public void Stop()
        {
            // Stop listeners
            Listeners.StopAll();

            // Everything have been stopped
            IsLiving = false;
        }
    }
}
