using System.Threading;

namespace LoriaNET
{
    /// <summary>
    /// A class providing a simple way to implement the listener interface.
    /// </summary>
    public abstract class Listener : Module, IListener
    {
        /// <summary>
        /// Time in milliseconds to wait between loop.
        /// </summary>
        protected int MillisecondsSleep { get; }

        /// <summary>
        /// Listener thread.
        /// </summary>
        protected Thread Thread { get; }

        /// <summary>
        /// Flag to know if the listener is running.
        /// </summary>
        protected bool IsRunning { get; set; }

        /// <summary>
        /// Flag to know if the listener is paused.
        /// </summary>
        protected bool IsPaused { get; set; }

        /// <summary>
        /// Create a new instance of listener.
        /// </summary>
        /// <param name="configuration">Loria's configuration.</param>
        /// <param name="millisecondsSleep">Time in milliseconds to wait between loop.</param>
        public Listener(Loria loria, int millisecondsSleep)
            : base(loria)
        {
            Loria = loria;
            MillisecondsSleep = millisecondsSleep;

            Thread = new Thread(Loop);
            IsRunning = false;
            IsPaused = false;
        }

        /// <summary>
        /// Start the listener.
        /// </summary>
        public virtual void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Thread.Start();
            }
        }

        /// <summary>
        /// Stop the listener.
        /// </summary>
        public virtual void Stop()
        {
            if (!IsRunning)
            {
                IsRunning = false;
                Thread.Join();
            }
        }

        /// <summary>
        /// Pause the listener.
        /// </summary>
        public virtual void Pause()
        {
            if (!IsPaused)
            {
                IsPaused = true;
            }
        }

        /// <summary>
        /// Resume the listener.
        /// </summary>
        public virtual void Resume()
        {
            if (IsPaused)
            {
                IsPaused = false;
            }
        }
        
        /// <summary>
        /// Main loop of listener.
        /// </summary>
        protected virtual void Loop()
        {
            while (IsRunning)
            {
                if (!IsPaused)
                {
                    Loria.Hub.Propagate(Listen());
                }

                Thread.Sleep(MillisecondsSleep);
            }
        }

        /// <summary>
        /// Method called inside listener main loop.
        /// </summary>
        /// <returns>A command or a message to propagate.</returns>
        public abstract string Listen();
    }
}
