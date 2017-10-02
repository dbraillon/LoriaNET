using LoriaNET.Storage;
using System.Threading;

namespace LoriaNET
{
    /// <summary>
    /// Base class to interact with Loria.
    /// </summary>
    public sealed class Loria
    {
        /// <summary>
        /// Flag to know if Loria is living.
        /// </summary>
        public bool IsLiving { get; private set; }

        /// <summary>
        /// Loria's configuration.
        /// </summary>
        public Data Data { get; }

        /// <summary>
        /// Every modules loaded by Loria.
        /// </summary>
        public Modules Modules { get; set; }

        /// <summary>
        /// Every actions loaded by Loria.
        /// </summary>
        public Actions Actions { get; }

        /// <summary>
        /// Every callbacks loaded by Loria.
        /// </summary>
        public Callbacks Callbacks { get; }

        /// <summary>
        /// Every listeners loaded by Loria.
        /// </summary>
        public Listeners Listeners { get; }

        /// <summary>
        /// Loria's hub, used to propagate commands or callbacks.
        /// </summary>
        public Hub Hub { get; }

        /// <summary>
        /// Create a new instance of Loria with a basic configuration.
        /// </summary>
        public Loria()
        {
            // Retrieve data
            Data = new Data();

            // Configure all modules
            Modules = new Modules(this);
            Modules.ConfigureAll();

            // Initialize actions, callbacks and listeners
            Actions = new Actions(Modules.GetAll<IAction>());
            Callbacks = new Callbacks(Modules.GetAll<ICallback>());
            Listeners = new Listeners(Modules.GetAll<IListener>());

            // Prepare the hub
            Hub = new Hub(this);
        }

        /// <summary>
        /// Start Loria and block the current thread.
        /// </summary>
        public void Live()
        {
            // Fallback to LiveAsync
            LiveAsync();

            // Block current thread
            while (IsLiving) Thread.Sleep(1000);
        }

        /// <summary>
        /// Start Loria and return as soon as everything has been set up.
        /// </summary>
        public void LiveAsync()
        {
            // Turn the flag on
            IsLiving = true;

            // Start enabled listeners
            Listeners.StartAll();
        }

        /// <summary>
        /// Stop Loria smoothly.
        /// </summary>
        public void Stop()
        {
            // Stop listeners
            Listeners.StopAll();

            // Everything have been stopped
            IsLiving = false;
        }
    }
}
