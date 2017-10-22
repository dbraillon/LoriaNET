using System;

namespace LoriaNET
{
    public interface IListener : IHasName
    {
        void Start();
        void Stop();
        void Pause();
        void Resume();

        event EventHandler<ListenerEventArgs> OnEvent;
    }
}
