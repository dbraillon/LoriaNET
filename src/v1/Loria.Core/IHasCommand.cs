namespace Loria.Core
{
    public interface IHasCommand : IHasName
    {
        string Command { get; }

        void Perform(Command command);
    }
}
