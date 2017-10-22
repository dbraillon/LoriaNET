namespace LoriaNET
{
    public interface IModule : IHasName
    {
        string Name { get; }

        bool IsEnabled();
        void Configure();
        void Activate();
        void Deactivate();
    }
}
