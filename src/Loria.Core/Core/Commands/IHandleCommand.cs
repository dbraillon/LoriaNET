namespace LoriaNET
{
    public interface IHandleCommand : IHasName
    {
        string Command { get; }

        void Perform(Command command);
    }
}
