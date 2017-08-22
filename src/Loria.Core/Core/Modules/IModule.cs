namespace LoriaNET
{
    /// <summary>
    /// An interface to make modules for Loria.
    /// </summary>
    internal interface IModule
    {
        string Name { get; }

        bool IsEnabled();
        void Configure();
        void Activate();
        void Deactivate();
    }
}
