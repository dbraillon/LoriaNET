namespace Loria.Core
{
    public interface IListener
    {
        string Name { get; }

        void Start();
        void Stop();
        void Pause();
        void Resume();
        string Listen();
    }
}
