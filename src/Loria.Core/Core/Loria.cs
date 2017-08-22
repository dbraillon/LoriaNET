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
        public Configuration Configuration { get; }

        /// <summary>
        /// Create a new instance of Loria with a basic configuration.
        /// </summary>
        public Loria() : this(new Configuration())
        {
        }

        /// <summary>
        /// Create a new instance of Loria.
        /// </summary>
        /// <param name="configuration">A custom configuration.</param>
        public Loria(Configuration configuration)
        {
            Configuration = configuration;
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
            Configuration.Listeners.StartAll();
        }

        /// <summary>
        /// Stop Loria smoothly.
        /// </summary>
        public void Stop()
        {
            // Stop listeners
            Configuration.Listeners.StopAll();

            // Everything have been stopped
            IsLiving = false;
        }
    }
}
