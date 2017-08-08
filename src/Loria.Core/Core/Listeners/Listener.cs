using System.Threading;

namespace Loria.Core
{
    public abstract class Listener : IListener
    {
        protected int MillisecondsSleep { get; }
        protected Configuration Configuration { get; }

        protected Thread Thread { get; }
        protected bool IsRunning { get; set; }
        protected bool IsPaused { get; set; }

        public abstract string Name { get; }

        public Listener(Configuration configuration, int millisecondsSleep)
        {
            Configuration = configuration;
            MillisecondsSleep = millisecondsSleep;

            Thread = new Thread(Loop);
            IsRunning = false;
            IsPaused = false;
        }

        public void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Thread.Start();
            }
        }
        public void Stop()
        {
            if (!IsRunning)
            {
                IsRunning = false;
                Thread.Join();
            }
        }
        public void Pause()
        {
            if (!IsPaused)
            {
                IsPaused = true;
            }
        }
        public void Resume()
        {
            if (IsPaused)
            {
                IsPaused = false;
            }
        }

        protected virtual void Loop()
        {
            while (IsRunning)
            {
                if (!IsPaused)
                {
                    Configuration.Hub(Listen());
                }

                Thread.Sleep(MillisecondsSleep);
            }
        }

        public abstract string Listen();
    }
}
