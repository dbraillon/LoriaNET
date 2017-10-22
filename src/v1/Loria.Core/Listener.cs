using System;
using System.Threading;

namespace Loria.Core
{
    public abstract class Listener : Module, IListener
    {
        protected int MillisecondsSleep { get; }
        protected Thread Thread { get; }
        protected bool IsRunning { get; set; }
        protected bool IsPaused { get; set; }
        
        public Listener(Engine engine, int millisecondsSleep)
            : base(engine)
        {
            MillisecondsSleep = millisecondsSleep;
            Thread = new Thread(Loop);
            IsRunning = false;
            IsPaused = false;
        }

        public virtual void Start()
        {
            if (!IsRunning)
            {
                IsRunning = true;
                Thread.Start();
            }
        }
        
        public virtual void Stop()
        {
            if (IsRunning)
            {
                IsRunning = false;
                Thread.Interrupt();
                Thread.Join();
            }
        }
        
        public virtual void Pause()
        {
            if (!IsPaused)
            {
                IsPaused = true;
            }
        }
        
        public virtual void Resume()
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
                    var result = Listen();
                    if (result != null)
                    {
                        Engine.Propagator.Propagate(result);
                    }
                }

                Thread.Sleep(MillisecondsSleep);
            }
        }

        protected abstract Command Listen();
    }
}
