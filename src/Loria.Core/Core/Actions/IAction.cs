namespace LoriaNET
{
    /// <summary>
    /// An interface to make actions for Loria.
    /// </summary>
    public interface IAction
    {
        string Name { get; }
        string Description { get; }

        string Command { get; }
        string[] SupportedIntents { get; }
        string[] SupportedEntities { get; }
        string[] Samples { get; }

        void Perform(Command command);
    }
}
