namespace LoriaNET
{
    /// <summary>
    /// An interface to make callbacks for Loria.
    /// </summary>
    internal interface ICallback
    {
        string Name { get; }

        void Callback(string message);
    }
}
