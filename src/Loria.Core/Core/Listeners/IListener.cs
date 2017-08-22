namespace LoriaNET
{
    /// <summary>
    /// An interface to make listeners for Loria.
    /// </summary>
    public interface IListener
    {
        string Name { get; }

        void Start();
        void Stop();
        void Pause();
        void Resume();
    }
}
