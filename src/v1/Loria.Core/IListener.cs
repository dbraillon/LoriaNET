using System;

namespace Loria.Core
{
    public interface IListener : IHasName
    {
        void Start();
        void Stop();
        void Pause();
        void Resume();
    }
}
