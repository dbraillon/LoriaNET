namespace Loria.Core
{
    public interface IModule : IHasName
    {
        bool IsEnabled();
        void Configure();
        void Activate();
        void Deactivate();
    }
}
