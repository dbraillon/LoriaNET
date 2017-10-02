namespace LoriaNET
{
    /// <summary>
    /// An interface to make callbacks for Loria.
    /// </summary>
    public interface ICallback
    {
        string Name { get; }

        void Callback(string message);
    }
}
